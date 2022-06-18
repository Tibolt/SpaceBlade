using Godot;
using System;

public class ChangeSceneButton : Button
{

    [Export]
    public PackedScene NextScene {get;set;}
    public override void _Ready()
    {
        if(NextScene == null)
            GD.Print("Next Scene is empty!");
        
    }

    public void OnButtonPressed()
    {
        if(NextScene != null)
        {
            GetTree().ChangeSceneTo(NextScene);
        }
    }

}
