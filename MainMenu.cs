using Godot;
using System;
using System.Collections.Generic;

public class MainMenu : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    PackedScene packedScene;
    public List<int> player_id = new List<int>();
    public int net_id;
    public void _on_Host_pressed()
    {
        var net = new NetworkedMultiplayerENet();
        net.CreateServer(5000, 2);
        GetTree().NetworkPeer = net;
    }

    public void _on_Join_pressed()
    {
        var net = new NetworkedMultiplayerENet();
        net.CreateClient("127.0.0.1", 5000);
        GetTree().NetworkPeer = net;
    }

    public void Success(int id)
    {
        Console.WriteLine(id);
        player_id.Add(id);
        if (player_id.Count > 0)
        {
            packedScene = (PackedScene)GD.Load("res://Base.tscn");
            Node2D node2D = (Node2D)packedScene.Instance();
            node2D.Position = new Vector2(0, 0);
            AddChild(node2D);
        }
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetTree().Connect("network_peer_connected", this, "Success");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
