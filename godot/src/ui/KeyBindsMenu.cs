using Godot;
using System;

public class KeyBindsMenu : Button
{

	private VBoxContainer _buttonContainer {get; set;}
	private Panel _panel {get; set;}
	static private Godot.Collections.Dictionary<string, uint> keybinds = new Godot.Collections.Dictionary<string, uint>();
	static private Godot.Collections.Dictionary<string, KeyButton> buttons = new Godot.Collections.Dictionary<string, KeyButton>();

	public override void _Ready()
	{
		_buttonContainer = GetNode<VBoxContainer>("CanvasLayer/Panel/VBoxContainer");
		_panel = GetNode<Panel>("CanvasLayer/Panel");
		keybinds = Globals.Keybinds.Duplicate();
		InitFunc();

	}

	private void InitFunc()
	{
		foreach(var key in keybinds.Keys)
		{
			var hbox = new HBoxContainer();
			var label = new Label();
			var button = new KeyButton();

			hbox.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
			label.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
			button.SizeFlagsHorizontal = (int)Control.SizeFlags.ExpandFill;
			
			label.Text = key;

			var buttonValue = keybinds[key];
			if(buttonValue == (uint)KeyList.Notsign)
				button.Text = "Unassigned";
			else
				button.Text = OS.GetScancodeString(buttonValue);
			
			button.Key = key;
			button.Value = buttonValue;
			button.Menu = this;
			button.ToggleMode = true;
			button.FocusMode = Control.FocusModeEnum.None;

			hbox.AddChild(label);
			hbox.AddChild(button);
			_buttonContainer.AddChild(hbox);
			
			buttons[key] = button;
		}
	}

	static public void ChangeBinds(string key, uint value)
	{
		keybinds[key] = value;
		foreach(var i in keybinds.Keys)
		{
			if(i != key && value != (uint)KeyList.Notsign && keybinds[i] == value)
			{
				keybinds[i] = (uint)KeyList.Notsign;
				buttons[i].Value = (uint)KeyList.Notsign;
				buttons[i].Text = "Unassigned";
			}
		}
	}

	private void OnBackPressed()
	{
		_panel.Visible = false;
	}


	private void OnSavePressed()
	{
		// Replace with function body.
		Globals.Keybinds = keybinds.Duplicate();
		Globals.SetKeyBinds();
		Globals.WriteConfig();
		OnBackPressed();
	}

	private void OnControlsPressed()
	{
		// Replace with function body.
		_panel.Visible = true;

	}
}


