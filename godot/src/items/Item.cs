using Godot;
using System;

public class Item : Node2D
{
    // public struct Values
    // {
    //     public int FrameNumber;
    //     public float ItemRarity;
    //     public int ItemValue;
    // }


    
    [Export]
    public String ItemName {get;set;}
    [Export]
    public int Value {get;set;}
    [Export]
    public int MaxSpeed {get;set;}
    // [Export]
    // public Godot.Collections.Dictionary<String, Vals> Items = new Godot.Collections.Dictionary<String, Vals>();
    public ItemDict Items = new ItemDict();
    
    public Vector2 Velocity {get;set;}
    private Sprite _sprite {get;set;}
    private CollisionShape2D _collision {get;set;}

    public override void _Ready()
    {
        _sprite = GetNode<Sprite>("Sprite");
        _collision = GetNode<CollisionShape2D>("Hurtbox/CollisionShape2D");

        AddItems();
        foreach(var name in Items)
        {
            GlobalVariables.ItemNames.Add(name.Key);
        }
        GlobalVariables.ItemsDict = Items;
    }

    public override void _Process(float delta)
    {
        Move(delta);
    }

    public void Move(float delta)
    {
        var direction = Vector2.Down;
        Position += direction * MaxSpeed * delta;
    }

    public void SetItemName(String name, int value = 0)
    {
        try
        {
            _sprite.Frame = Items[name].FrameNumber;
            ItemName = name;
            Value = value;
        }
        catch
        {
            GD.Print("SetItemName not working");
        }
    }

    public void SetCollisionShape(int radius)
    {
        var circle = new CircleShape2D();
        circle.Radius = radius;
        _collision.Shape = circle;
    }

    public void OnTimerTimeout()
    {
        QueueFree();
    }

    public void OnHurtboxAreaEntered(Area2D area)
    {
        QueueFree();
    }
    
    public void AddItems()
    {
        //TODO: read items from file ?
        Items.Add("Coin", new Vals(0, 0.55f, 10));
        Items.Add("Skull", new Vals(1, 0.85f, 2));
        Items.Add("Armor", new Vals(2, 0.98f, 1));
        Items.Add("Bullet", new Vals(3, 0.8f, 1));
        Items.Add("Speed", new Vals(4, 0.65f, 10));
        Items.Add("Health", new Vals(5, 0.95f, 1));
    }
}
