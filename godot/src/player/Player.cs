using Godot;
using System;

public class Player : Ship
{
    public enum States
    {
        MOVE,
        DEAD,
    };

    public States State = States.MOVE;
    public bool IsReloading = false;
    [Export]
    public float ReloadTime = 1.5f;
    [Export]
    public int NumberOfBullets = 5;
    public int BulletsShooted {get;set;}


    private PackedScene _bullet = (PackedScene)ResourceLoader.Load("res://src/player/PlayerBullet.tscn");
    private Position2D _shootPoint {get; set;}
    private Timer _reloadTimer {get;set;}
    public override void _Ready()
    {
        _shootPoint = GetNode<Position2D>("ShootPoint");   
        _reloadTimer = GetNode<Timer>("ReloadTimer");
        BulletsShooted = NumberOfBullets;
    }
    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("reload"))
            Reload();
    }

    public override void _Process(float delta)
    {
        WrapEdges();

        switch(State)
        {
            case States.MOVE:
                MoveState(delta);
                break;
            case States.DEAD:
                DeadState();
                break;
        }

        if(Input.IsActionJustPressed("shoot") && !IsReloading)
        {
            if(BulletsShooted <= 0)
            {
                Reload();
            }
            else
            {
                InstanceBullet();
                --BulletsShooted;
            }
        }
    }
    public Vector2 GetDirection()
    {
        return new Vector2(Input.GetActionStrength("right") - Input.GetActionStrength("left"), 0);
    }

    public void MoveState(float delta)
    {
        Vector2 direction = GetDirection();
        direction = direction.Normalized();

        if(direction != Vector2.Zero)
        {
            Velocity = Velocity.MoveToward(direction * MaxSpeed, Acceleration * delta);
        }
        else
        {
            Velocity = Velocity.MoveToward(Vector2.Zero, Friction * delta);
        }

        Velocity = MoveAndSlide(Velocity);

    }

    public void DeadState()
    {
        // play animation, and restart game
    }

    public void InstanceBullet()
    {
        var bulletInstance = _bullet.Instance<PlayerBullet>();
        bulletInstance.Position = _shootPoint.GlobalPosition;

        GetParent().AddChild(bulletInstance);
    }

    public void OnHurtboxAreaEntered(Hitbox hitbox)
    {
        // if(Health <= 0) 
            // State = States.DEAD;
        GD.Print("Player - 1HP");
    }

    public void OnReloadTimerTimeout()
    {
        IsReloading = false;
        BulletsShooted = NumberOfBullets;
    }

    public void OnItemCollectorAreaEntered(Hurtbox hurtbox)
    {
        var item = hurtbox.GetParent<Item>();
        if(item != null)
            ItemEffects(item.ItemName);
    }

    public void ItemEffects(string name)
    {
        switch(name)
            {
                case "Health":
                    Health++;
                    GD.Print("Health: " + Health);
                    break;
                case "Armor":
                    break;
                case "Speed":
                    Acceleration += 5;
                    GD.Print("Acceleration: " + Acceleration);
                    break;
                case "Bullet":
                    NumberOfBullets++;
                    break;
                case "Coin":
                    GlobalVariables.Money += 10;
                    break;
                default:
                    GD.Print(name);
                    break;
            }
    }

    public void WrapEdges()
    {
        if(Position.x > GlobalVariables.ScreenRight)
            Position = new Vector2(GlobalVariables.ScreenLeft, Position.y);
        if(Position.x < GlobalVariables.ScreenLeft)
            Position = new Vector2(GlobalVariables.ScreenRight, Position.y);
    }

    public void Reload()
    {
        _reloadTimer.Start(ReloadTime);
        IsReloading = true;
    }
}
