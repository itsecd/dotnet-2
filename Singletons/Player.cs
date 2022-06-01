using Godot;

public class Player : Node
{
    public int _playerId;
    public ulong _requester;
    public bool _isPlaying;
    public int _hp;

    public Player(int i, bool b, ulong req, int hp)
    {
        _playerId = i;
        _isPlaying = b;
        _requester = req;
        _hp = hp;
    }

    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
