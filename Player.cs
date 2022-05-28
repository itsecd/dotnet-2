using Godot;
using System;

public class Player : Node2D
{
    private int[] state;
    private AnimatedSprite[] spriteList;
    public bool[] isPressed;
    private int collision = -1;

    public void _on_Song_Beat(int beat)
    {
        collision = beat % 8;
        Console.WriteLine("Shit working fine");
    }
    private void StateChanger(int collision)
    {
        if (Input.IsActionJustPressed("UpAttack") && !isPressed[collision])
        {
            state[collision] = 1;
            spriteList[collision].Frame = 1;
            isPressed[collision] = true;
            collision = -1;
        }
        else if (Input.IsActionJustPressed("DownAttack") && !isPressed[collision])
        {
            state[collision] = 2;
            spriteList[collision].Frame = 2;
            isPressed[collision] = true;
            collision = -1;
        }
        else
            collision = -1;

    }

    public override void _Ready()
    {
        state = new int[8];
        spriteList = new AnimatedSprite[8];
        isPressed = new bool[8];
        for (int i = 1; i <= 8; ++i)
        {
            spriteList[i - 1] = (AnimatedSprite)GetNode("Player1/HitBox" + i);
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if(collision != -1)
            StateChanger(collision);
    }
}

