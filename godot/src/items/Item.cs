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
    [Export]
    public Godot.Collections.Dictionary<String, int> Items = new Godot.Collections.Dictionary<String, int>();
    
    public Vector2 Velocity {get;set;}
    public Sprite _sprite {get;set;}

    // public Item(string name, int value)
    // {
    //     ItemName = name;
    //     Value = value;
    // }
    public override void _Ready()
    {
        _sprite = GetNode<Sprite>("Sprite");

        foreach(var name in Items)
        {
            GlobalVariables.ItemNames.Add(name.Key);
        }
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
            _sprite.Frame = Items[name];
            ItemName = name;
            Value = value;
        }
        catch
        {
            GD.Print("SetItemName not working");
        }

    }

    public void OnTimerTimeout()
    {
        QueueFree();
    }


}
