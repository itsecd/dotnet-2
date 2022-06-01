using Godot;
using System;
using System.Linq;

public class Player : Node2D
{
    [Signal] public delegate void Ready();
    [Signal] public delegate void Gameover();
    [Signal] public delegate void Victory();

    private int[] _state;
    private int[] _puppetState = {0};
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
    }

    private void MessageFormatter(string mes, int[] state)
    {
        for (int i = 0; i < state.Length; ++i)
        {
            mes += state[i].ToString();
            if (i != 7)
                mes += ";";
        }
    }
    public void SetEnemy(int enemy)
    {
        _enemy = enemy;
    }
    public void _on_Base_Hosted()
    {
        Console.WriteLine("Shit working fine host");
        GetNode<Server>("/root/Server").SendHostData(GetInstanceId());
    }

    public void _on_Base_Client()
    {
        Console.WriteLine("Shit working fine");
        GetNode<Server>("/root/Server").FindMatchup(GetInstanceId());
    }
    public void _on_Song_Beat(int beat)
    {
        _collision = beat % 8;
    }
    private void StateChanger()
    {
        for (int i = 0; i < _state.Length; ++i)
        {
            _state[i] = 0;
            //puppetSpriteList[i].Frame = puppet_state[i];
        }
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
                string mes = "";
                for (int i = 0; i < _state.Length; ++i)
                {
                    mes += _state[i].ToString();
                    if (i != 7)
                        mes += ";";
                }
                GetNode<Server>("/root/Server").SendAttack(mes, GetInstanceId(), _enemy);
                GetNode<Timer>("Cooldown").Start();
            }
            else
            {
                string mesAttack = "";
                string mesDefence = "";
                for (int i = 0; i < _state.Length; ++i)
                {
                    mesAttack += _state[i].ToString();
                    if (i != 7)
                        mesAttack += ";";
                }
                for (int i = 0; i < _puppetState.Length; ++i)
                {
                    mesDefence += _puppetState[i].ToString();
                    if (i != 7)
                        mesDefence += ";";
                }
                MessageFormatter(mesDefence, _puppetState);
                GetNode<Server>("/root/Server").SendDefence(mesDefence, mesAttack, GetInstanceId());
                GetNode<Timer>("Cooldown").Start();
            }
            StateChanger();
        }
        collision = -1;

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

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
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

