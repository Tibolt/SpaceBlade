using Godot;
using System;

public class Item : Node2D
{
    [Export]
    public String ItemName {get;set;}
    [Export]
    public int Value {get;set;}
    [Export]
    public TextureRect Txt {get;set;}
    [Export]
    public int MaxSpeed {get;set;}
    
    public Vector2 Velocity {get;set;}
    private Sprite _sprite {get;set;}
    public override void _Ready()
    {
        _sprite = GetNode<Sprite>("Sprite");

        // try
        // {
        //     _sprite.Texture = Txt.Texture;
        // }
        // catch (System.Exception)
        // {
        //     GD.Print("Missing Texture");
        //     throw;
        // }
    }

    public override void _Process(float delta)
    {
        Move(delta);
    }

    public void Move(float delta)
    {
        var direction = Vector2.Down;

        // Velocity = Velocity.MoveToward(direction * MaxSpeed, MaxSpeed * delta);

        Position += direction * MaxSpeed * delta;

    }

    public void OnTimerTimeout()
    {
        QueueFree();
    }


}
