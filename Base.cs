using Godot;
using System;

public class Base : Node2D
{
    [Export]public bool _isHost = false;
    [Signal] public delegate void Hosted();
    [Signal] public delegate void Client();
    [Signal] public delegate void Freeze();
    [Signal] public delegate void Unfreeze();
    private bool _isFreeze = false;
    public void BoxCleaner()
    {
        for (int i = 1; i <= 8; ++i)
        {
            GetNode<Player>("Player1")._isPressed[i - 1] = false;
            GetNode<AnimatedSprite>("Player1/Player1/HitBox" + i).Frame = 0;

        }
        GetNode<Timer>("Song/Cooldown").Start();
    }

    public void _on_Player_Gameover()
    {
        GetNode<Song>("Song").Playing = false;
        GetNode<Player>("Player1")._freeze = true;
        GetNode<Panel>("Panel").Visible = true;
        GetNode<Label>("Panel/Label").Visible = true;
        GetNode<Label>("Panel/Label2").Visible = false;
    }
    public void _on_Player_Victory()
    {
        GetNode<Song>("Song").Playing = false;
        GetNode<Player>("Player1")._freeze = true;
        GetNode<Panel>("Panel").Visible = true;
        GetNode<Label>("Panel/Label").Visible = false;
        GetNode<Label>("Panel/Label2").Visible = true;
    }
    public override void _Ready()
    {
        GetNode<Timer>("Song/Cooldown").OneShot = true;
        if (_isHost)
            EmitSignal("Hosted");
        else
            EmitSignal("Client");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var anal = GetNode<Song>("Song")._songPositionInBeats % 8;
        var anal2 = GetNode<Song>("Song")._songPositionInBeats % 16;
        if (anal2 == 0 && GetNode<Timer>("Song/Cooldown").IsStopped())
        {
            GetNode<Player>("Player1")._freeze = false;
            BoxCleaner();
        }
        if (anal == 0 && GetNode<Timer>("Song/Cooldown").IsStopped())
        {
            Console.WriteLine("Otdihaem Otdihaem Otdihaem Otdihaem Otdihaem Otdihaem Otdihaem ");
            GetNode<Player>("Player1")._freeze = true;
            BoxCleaner();
        }
    }
}
