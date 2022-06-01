using Godot;
using System;
using System.Collections.Generic;

public class MainMenu : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    PackedScene _packedScene;
    public void _on_Host_pressed()
    {
        _packedScene = (PackedScene)GD.Load("res://Base.tscn");
        Base node2D = (Base)_packedScene.Instance();
        node2D.Position = new Vector2(0, 0);
        node2D._isHost = true;
        AddChild(node2D);
    }

    public void _on_Join_pressed()
    {
        _packedScene = (PackedScene)GD.Load("res://Base.tscn");
        Base node2D = (Base)_packedScene.Instance();
        node2D.Position = new Vector2(0, 0);
        node2D._isHost = false;
        AddChild(node2D);
        GetNode<Button>("Host").Disabled = true;
        GetNode<Button>("Join").Disabled = true;
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
