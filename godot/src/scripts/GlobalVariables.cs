using Godot;
using System;

public class GlobalVariables : Node2D
{
    static public int Score {set;get;}
    static public bool IsPaused {set;get;}
    static public float ScreenLeft {set;get;}
    static public float ScreenRight {set;get;}
    static public float ScreenTop {set;get;}
    public const float Border = 100;
    static public Godot.Collections.Array<String> ItemNames = new Godot.Collections.Array<String>();
    public override void _Ready()
    {
        Score = 0;
        IsPaused = false;   

        var rect = GetViewportRect();
        ScreenLeft = rect.Position.x;
        ScreenRight = rect.Position.x + rect.Size.x;
        ScreenTop = rect.Position.y;
    }


}
