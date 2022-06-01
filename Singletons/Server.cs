using Godot;
using System;
using System.Collections.Generic;

public class Server : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private NetworkedMultiplayerENet _network = new NetworkedMultiplayerENet();
    private int _port = 4242;
    private int _maxPlayers = 100;
    private List<Player> _players;
    // Called when the node enters the scene tree for the first time.

    private void StartServer()
    {
        _network.CreateServer(_port, _maxPlayers);
        GetTree().NetworkPeer = _network;
        Console.WriteLine("Server started");
        _network.Connect("peer_connected", this, "OnPeerConnected");
        _network.Connect("peer_disconnected", this, "OnPeerDisconnected");
    }

    private void OnPeerConnected(int id)
    {
        Console.WriteLine("User " + id + "is connected");
    }

    private void OnPeerDisconnected(int id)
    {
        Console.WriteLine("User " + id + "is disconnected");
        _players.Find(p => p._playerId == id).Dispose();
    }
    [Remote]
    private void ReceiveAttack(string attack, ulong requester, int target)
    {
        Console.WriteLine("Attack " + attack + " received!");
        Console.WriteLine("Requester: " + requester);
        RpcId(target, "ReceiveAttack", attack);
    }
    [Remote]
    private void ReceiveDefence(string defence, string attack, ulong requester)
    {
        var counter = 0;
        var damage = 0;
        Player player = _players.Find(p => p._requester == requester);
        Console.WriteLine("Defence " + defence + " for " + attack + "received!");
        Console.WriteLine("Requester: " + requester);
        for (int i = 0; i < attack.Length; ++i)
        {
            if (defence[i] != attack[i])
            {
                counter++;
            }
        }
        damage = counter * 11;
        RpcId(player._playerId, "ReceiveDefence", damage);
    }
    [Remote]
    private void ReceiveData(ulong requester)
    {
        _players.Add(new Player(GetTree().GetRpcSenderId(), false, requester, 100));
    }
    [Remote]
    private void ReceiveGameover(ulong requester, int target)
    {
        RpcId(target, "ReceiveGameover");
    }
    [Remote]
    private void FindMatchup(ulong requester)
    {
        Player player = _players.Find(p => p._isPlaying == false);
        _players.Add(new Player(GetTree().GetRpcSenderId(), true, requester, 100));
        RpcId(player._playerId, "ReceiveMatchup", GetTree().GetRpcSenderId(), player._requester, 100);
        RpcId(GetTree().GetRpcSenderId(), "ReceiveMatchup", player._playerId, requester, 100);
        Console.WriteLine("For " + player._requester + " enemy is: " + GetTree().GetRpcSenderId());
        Console.WriteLine("For " + GetTree().GetRpcSenderId() + " enemy is: " + player._playerId);
        _players.Find(p => p._requester == player._requester)._isPlaying = true;
    }
    public override void _Ready()
    {
        StartServer();
        _players = new List<Player>();
    }


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
