using Godot;
using System;

public class Globals : Node
{
    public Node CurrentScene {get;set;}
    static public int Score {set;get;}
    public override void _Ready()
    {
        Viewport root = GetTree().GetRoot();
        CurrentScene = root.GetChild(root.GetChildCount()-1);
    }

    public void GoToScene(string path)
    {
        CallDeferred(nameof(DeferredGoToScene), path);
    }

    public void DeferredGoToScene(string path)
    {
        CurrentScene.Free();

        var nextScene = (PackedScene)GD.Load(path);
        CurrentScene = nextScene.Instance();
        GetTree().GetRoot().AddChild(CurrentScene);

        GetTree().SetCurrentScene(CurrentScene);
    }

    public void InstanceScene(string path)
    {
        CurrentScene.Free();

        var scene = (PackedScene)GD.Load(path);
        CurrentScene = scene.Instance();
        GetTree().GetRoot().AddChild(CurrentScene);
    }

    public void GoScene(string path)
    {
        var scene = (PackedScene)GD.Load(path);
        GetTree().ChangeSceneTo(scene);
    }


}
