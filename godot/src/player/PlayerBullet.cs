using Godot;
using System;

public class PlayerBullet : Node2D
{
    [Export]
    public Vector2 Direction = Vector2.Up;
    [Export]
    public float MaxSpeed {get; set;}
    [Export]
    public float Acceleration {get; set;}
    [Export]
    public int Damage {get; set;}


    public override void _Ready()
    {
        Visible = true;
    }

    public override void _Process(float delta)
    {
        GlobalPosition += Direction * MaxSpeed * delta;
    }

    public void Destory()
    {
        QueueFree();
    }

    public void OnTimerTimeout()
    {
        Destory();
    }


}
