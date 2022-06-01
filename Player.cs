using Godot;
using System;
using System.Linq;

public class Player : Node2D
{
    [Signal] public delegate void Ready();
    [Signal] public delegate void Gameover();
    [Signal] public delegate void Victory();

    private int[] _state;
    private int[] _puppetState = { 0 };
    private AnimatedSprite[] _spriteList;
    private AnimatedSprite[] _puppetSpriteList;
    public bool[] _isPressed;
    private int _collision = -1;
    private int _enemy;
    public int _hp = 0;
    public int _dummyHp;
    public bool _freeze = true;
    private bool _attackMod = true;
    private bool _gameover = false;


    public void SetPuppetAttack(string attack)
    {
        _puppetState = attack.Split(';').Select(int.Parse).ToArray();
        for (int i = 0; i < _state.Length; ++i)
            _puppetSpriteList[i].Frame = _puppetState[i];
    }

    public void SetGameMode(bool gamemode)
    {
        _attackMod = gamemode;
    }

    public void ApplyDamage(int damage)
    {
        _hp -= damage;
        GetNode<Label>("Info/Hp").Text = "HP: " + _hp;
    }

    private string MessageFormatter(int[] state)
    {
        var mes = "";
        for (int i = 0; i < state.Length; ++i)
        {
            mes += state[i].ToString();
            if (i != 7)
                mes += ";";
        }
        return mes;
    }

    public void SetEnemy(int enemy)
    {
        _enemy = enemy;
        GetNode<Label>("Info/Hp").Text = "HP: " + _hp;
    }

    public void _on_Base_Hosted()
    {
        GetNode<Server>("/root/Server").SendHostData(GetInstanceId());
    }

    public void _on_Base_Client()
    {
        GetNode<Server>("/root/Server").FindMatchup(GetInstanceId());
    }

    public void _on_Song_Beat(int beat)
    {
        _collision = beat % 8;
    }

    private void StateChanger()
    {
        for (int i = 0; i < _state.Length; ++i)
            _state[i] = 0;
    }

    private void SpriteChanger()
    {
        for (int i = 1; i <= 8; ++i)
        {
            _spriteList[i - 1] = (AnimatedSprite)GetNode("Player1/HitBox" + i);
            _puppetSpriteList[i - 1] = (AnimatedSprite)GetNode("Player2/HitBox" + i);
        }
    }

    public void WinCondition()
    {
        EmitSignal("Victory");
    }

    private void Gameplay(int collision)
    {
        if (Input.IsActionJustPressed("UpAttack") && !_isPressed[collision])
        {
            _state[collision] = 1;
            _spriteList[collision].Frame = 1;
            _isPressed[collision] = true;
        }
        else if (Input.IsActionJustPressed("DownAttack") && !_isPressed[collision])
        {
            _state[collision] = 2;
            _spriteList[collision].Frame = 2;
            _isPressed[collision] = true;
        }
        if (collision == 7 && GetNode<Timer>("Cooldown").IsStopped())
        {
            if (_attackMod)
            {
                var mes = MessageFormatter(_state);
                GetNode<Server>("/root/Server").SendAttack(mes, GetInstanceId(), _enemy);
                GetNode<Timer>("Cooldown").Start();
            }
            else
            {
                var mesAttack = MessageFormatter(_state);
                var mesDefence = MessageFormatter(_puppetState);
                GetNode<Server>("/root/Server").SendDefence(mesDefence, mesAttack, GetInstanceId());
                GetNode<Timer>("Cooldown").Start();
            }
            StateChanger();
        }
        collision = -1;
    }

    private void GuiChanger()
    {
        if (_attackMod)
        {
            GetNode<ColorRect>("Info/DefenceColor").Visible = false;
            GetNode<Label>("Info/Defence").Visible = false;
            GetNode<ColorRect>("Info/AttackColor").Visible = true;
            GetNode<Label>("Info/Attack").Visible = true;
        }
        else
        {
            GetNode<ColorRect>("Info/AttackColor").Visible = false;
            GetNode<Label>("Info/Attack").Visible = false;
            GetNode<ColorRect>("Info/DefenceColor").Visible = true;
            GetNode<Label>("Info/Defence").Visible = true;
        }
    }

    public override void _Ready()
    {
        _state = new int[8];
        _puppetState = new int[8];
        _spriteList = new AnimatedSprite[8];
        _puppetSpriteList = new AnimatedSprite[8];
        _isPressed = new bool[8];
        SpriteChanger();
    }

    public override void _Process(float delta)
    {
        GuiChanger();
        if (_hp < 0)
        {
            if (!_gameover)
            {
                EmitSignal("Gameover");
                GetNode<Server>("/root/Server").SendGameover(GetInstanceId(), _enemy);
                _gameover = true;
            }
        }
        if (!_freeze)
        {
            if (_collision != -1)
                Gameplay(_collision);
        }
    }
}

