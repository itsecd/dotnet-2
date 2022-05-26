using Godot;
using System;

public class Player : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private int[] state;
    Timer timer;
    private AnimatedSprite[] spriteList;
    private bool[] isPressed;
    private int collision = 0;
    // Called when the node enters the scene tree for the first time.

    private void StateChanger(int collision)
    {
        if (Input.IsActionPressed("UpAttack") && !isPressed[collision])
        {
            state[collision] = 1;
            spriteList[collision].Frame = 1;
            isPressed[collision] = true;
        }
        else if (Input.IsActionPressed("DownAttack") && !isPressed[collision])
        {
            state[collision] = 2;
            spriteList[collision].Frame = 2;
            isPressed[collision] = true;
        }
    }
    public override void _Ready()
    {
        timer = (Timer)GetNode("Player1/Timer");
        timer.Start();
        state = new int[8];
        spriteList = new AnimatedSprite[8];
        isPressed = new bool[8];
        for (int i = 1; i <= 8; i++)
        {
            spriteList[i - 1] = (AnimatedSprite)GetNode("Player1/HitBox" + i);
        }
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (!timer.IsStopped())
        {
            switch (collision)
            {
                case 0:
                    StateChanger(collision);
                    break;
                case 1:
                    StateChanger(collision);
                    break;
                case 2:
                    StateChanger(collision);
                    break;
                case 3:
                    StateChanger(collision);
                    break;
                case 4:
                    StateChanger(collision);
                    break;
                case 5:
                    StateChanger(collision);
                    break;
                case 6:
                    StateChanger(collision);
                    break;
                case 7:
                    StateChanger(collision);
                    break;
                default:
                    break;
            }
        }
    }
}

