using Godot;
using System;

public class ChangeSceneButton : Button
{

    [Export]
    public PackedScene NextScene {get;set;}
    private Globals _globals {get;set;}
    public override void _Ready()
    {
        if(NextScene == null)
            GD.Print("Next Scene is empty!");
        _globals = GetNode<Globals>("/root/Globals");
        
    }

    public void OnButtonPressed()
    {
        if(NextScene != null)
        {
            //TODO: add scene to tree, so later you can switch between both scenes.
            GetTree().ChangeSceneTo(NextScene);
            // _globals.GoToScene(NextScene.GetPath());
        }
    }

}
