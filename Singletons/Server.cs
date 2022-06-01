using Godot;
using System;

public class Server : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private Player PlayerInstance;
    private NetworkedMultiplayerENet _network = new NetworkedMultiplayerENet();
    private string _ip = "127.0.0.1";
    private int _port = 4242;

    private void ConnectToServer()
    {
        _network.CreateClient(_ip, _port);
        GetTree().NetworkPeer = _network;

        _network.Connect("connection_failed", this, "OnConnectionFailed");
        _network.Connect("connection_succeeded", this, "OnConnectionSucceeded");
    }

    private void OnConnectionFailed()
    {
        Console.WriteLine("Connection failed");
    }

    private void OnConnectionSucceeded()
    {
        Console.WriteLine("Connection succeeded");
    }
    public void SendAttack(string attack, ulong requester, int target)
    {
        RpcId(1, "ReceiveAttack", attack, requester, target);
    }
    public void SendDefence(string defence, string attack, ulong requester)
    {
        RpcId(1, "ReceiveDefence", defence, attack, requester);
    }
    public void SendHostData(ulong requester)
    {
        RpcId(1, "ReceiveData", requester);
    }

    public void FindMatchup(ulong requester)
    {
        RpcId(1, "FindMatchup", requester);
    }

    public void SendGameover (ulong requester, int target)
    {
        RpcId(1, "ReceiveGameover", requester, target);
    }
    [Remote]
    public void ReceiveMatchup(int enemy, ulong requester, int hp)
    {
        PlayerInstance = (Player)GD.InstanceFromId(requester);
        Console.WriteLine("Enemy: " + enemy + " is set for " + requester + " instance.");
        PlayerInstance.SetEnemy(enemy);
        PlayerInstance._freeze = false;
        PlayerInstance._hp = hp;
        PlayerInstance.EmitSignal("Ready");
    }
    [Remote]
    public void ReceiveGameover()
    {
        PlayerInstance.WinCondition();
    }

    [Remote]
    public void ReceiveAttack(string attack)
    {
        Console.WriteLine("Attack: " + attack + " received succesfully");
        PlayerInstance.SetPuppetAttack(attack);
        PlayerInstance.SetGameMode(false);
        Console.WriteLine("Gamemode is now: defence") ;

    }
    [Remote]
    public void ReceiveDefence(int damage)
    {
        Console.WriteLine("Damage taken: " + damage);
        PlayerInstance.ApplyDamage(damage);
        PlayerInstance.SetGameMode(true);
        Console.WriteLine("Gamemode is now: attack");
    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ConnectToServer();
        GetInstanceId();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
