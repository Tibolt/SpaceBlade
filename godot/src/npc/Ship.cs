using Godot;
using System;

public class Ship : KinematicBody2D
{
    [Export]
    public float MaxSpeed {get; set;}
    [Export]
    public float Acceleration {get; set;}
    [Export]
    public float Friction {get; set;}
    [Export]
    public int Health {get; set;}

    public Vector2 Velocity = Vector2.Zero;



    public override void _Ready()
    {
        
    }
}
