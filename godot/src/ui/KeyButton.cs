using Godot;
using System;

public class KeyButton : Button
{
    public string Key {get; set;}
    public uint Value {get; set;}
    public KeyBindsMenu Menu {get; set;}
    public bool WaitngInput {get; set;}

	public override void _Ready()
    {

    }

    public override void _Toggled(bool buttonPressed)
    {
        base._Toggled(buttonPressed);
        if(buttonPressed)
        {
            WaitngInput = true;
            this.Text = "Press Any Key";
        }
    }

    public override void _Input(InputEvent @event)
    {
        if(WaitngInput)
        {
            if(@event is InputEventKey eventKey)
            {
                Value = eventKey.Scancode;
                this.Text = OS.GetScancodeString(Value);
                KeyBindsMenu.ChangeBinds(Key, Value);

                this.Pressed = false;
                WaitngInput = false;
            }
            if(@event is InputEventMouseButton eventMouse)
            {
                if(Value != (uint)KeyList.Notsign)
                    this.Text = OS.GetScancodeString(Value);
                else
                    this.Text = "Unassigned";

                this.Pressed = false;
                WaitngInput = false;
            } 
        }
    }
    
    

}
