using Godot;
using System;

public class Base : Node2D
{

    public void BoxCleaner()
    {
        for (int i = 1; i <= 8; ++i)
        {
            GetNode<Player>("Player1").isPressed[i - 1] = false;
            GetNode<AnimatedSprite>("Player1/Player1/HitBox" + i).Frame = 0;

        }
        GetNode<Timer>("Song/Cooldown").Start();
    }
    public override void _Ready()
    {
        GetNode<Timer>("Song/Cooldown").OneShot = true;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var anal = GetNode<Song>("Song")._songPositionInBeats % 8;
        if(anal == 0 && GetNode<Timer>("Song/Cooldown").IsStopped())
        {
            BoxCleaner();
        }
    }
}
