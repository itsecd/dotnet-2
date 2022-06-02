using Godot;

public class ServerData : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    JSONParseResult _skillData;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var _skillDataFile = new File();
        _skillDataFile.Open("res://Data/IdFromAttacks", File.ModeFlags.Read);
        var _skillDataJson = JSON.Parse(_skillDataFile.GetAsText());
        _skillDataFile.Close();
        _skillData = (JSONParseResult)_skillDataJson.Result;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
