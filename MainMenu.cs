using Godot;

public class MainMenu : Control
{
    private PackedScene _packedScene;

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

    public override void _Ready()
    {
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
