using Godot;
using System;

public class Player : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    public int _playerId;
    public ulong _requester;
    public bool _isPlaying;
    public int _hp;

    public Player (int i, bool b, ulong req, int hp)
    {
        _playerId = i;
        _isPlaying = b;
        _requester = req;
        _hp = hp;
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
