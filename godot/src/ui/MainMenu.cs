using Godot;
using System;

public class MainMenu : Control
{
    public override void _Ready()
    {
        
    }


    public void OnExitButtonPressed()
    {
        GetTree().Quit();
    }
}
