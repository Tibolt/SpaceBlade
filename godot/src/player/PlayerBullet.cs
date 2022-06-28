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
    [Export]
    public bool IsPlayerBullet {get;set;}


    public override void _Ready()
    {
        Visible = true;
    }

    public override void _Process(float delta)
    {
        Destory();
        GlobalPosition += Direction * MaxSpeed * delta;
    }

    public void Destory()
    {
        if(IsPlayerBullet)
        {
            if(Position.y <= GlobalVariables.ScreenTop)
                QueueFree();
        }
        else
        {
            if(Position.y >= GlobalVariables.ScreenBottom)
                QueueFree();
        }
    }

    public void OnTimerTimeout()
    {
        // TODO: Delete Timer
        // Destory();
    }

    public void OnHitboxAreaEntered(Hurtbox hurtbox)
    {
        QueueFree();
    }


}
