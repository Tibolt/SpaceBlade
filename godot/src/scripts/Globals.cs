using Godot;
using System;

public class Globals : Node
{
	public Node CurrentScene {get;set;}
	static public int Score {set;get;}

	static private string _filepath = "res://keybinds.ini";
	static private ConfigFile _configFile {set; get;}
	static public Godot.Collections.Dictionary<string, uint> Keybinds = new Godot.Collections.Dictionary<string, uint>();

	public override void _Ready()
	{
		Viewport root = GetTree().GetRoot();
		CurrentScene = root.GetChild(root.GetChildCount()-1);

		ConfigInit();
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

	private void ConfigInit()
	{
		_configFile = new ConfigFile();
		if(_configFile.Load(_filepath) == Godot.Error.Ok)
		{
			foreach(var key in _configFile.GetSectionKeys("keybinds"))
			{
				var keyValue =_configFile.GetValue("keybinds", key);
				var intValue = Convert.ToUInt32(keyValue);
				GD.Print(key + " : " + OS.GetScancodeString(intValue));

				Keybinds[key] = intValue;
			}
		}
		else
		{
			GD.Print("Config file now found!");
			GetTree().Quit();
		}

		SetKeyBinds();
	}
	static public void SetKeyBinds()
	{
		foreach(string key in Keybinds.Keys)
		{
			var value = Keybinds[key];
			var actionList = InputMap.GetActionList(key);
			
			if(actionList.Count != 0)
			{
				InputEvent input = (InputEvent)actionList[0];
				InputMap.ActionEraseEvent(key, input);
			}
			
			var newKey = new InputEventKey();
			newKey.Scancode = value;
			InputMap.ActionAddEvent(key, newKey);
		}
	}

	static public void WriteConfig()
	{
		foreach(var key in Keybinds.Keys)
		{
			var keyValue = Keybinds[key];
			_configFile.SetValue("keybinds", key, keyValue);
		}
		_configFile.Save(_filepath);
	}

}
