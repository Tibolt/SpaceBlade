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


    private PackedScene _playerBullet = (PackedScene)ResourceLoader.Load("res://src/player/PlayerBullet.tscn");
    private Position2D _shootPoint {get; set;}
    public override void _Ready()
    {
        _shootPoint = GetNode<Position2D>("ShootPoint");   
    }

    public override void _Process(float delta)
    {
        if(Health <= 0) State = States.DEAD;

        switch(State)
        {
            case States.MOVE:
                MoveState(delta);
                break;
            case States.DEAD:
                DeadState();
                break;
        }

        if(Input.IsActionJustPressed("shoot"))
        {
            InstanceBullet();
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
        var playerBulletInstance = _playerBullet.Instance<PlayerBullet>();
        playerBulletInstance.Position = _shootPoint.GlobalPosition;

        // GD.Print("Bullet:" + playerBulletInstance.GlobalPosition);
        // GD.Print("ShootPoint:" + _shootPoint.GlobalPosition);
        // GD.Print("Player:" + GlobalPosition);

        GetParent().AddChild(playerBulletInstance);
    }
}
