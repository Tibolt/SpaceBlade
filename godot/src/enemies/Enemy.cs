using Godot;
using System;

public class Enemy : Ship
{
    [Export]
    public bool CanStrafe = false;
    private Sprite _sprite {get;set;}
    private CollisionShape2D _collisionShape {get;set;}
    private Hurtbox _hurtbox = null;
    private Position2D _shootPoint {get; set;}
    private PackedScene _bullet = (PackedScene)ResourceLoader.Load("res://src/enemies/EnemyBullet.tscn");
    public override void _Ready()
    {
        _sprite = GetNode<Sprite>("Sprite");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        _hurtbox = GetNode<Hurtbox>("Hurtbox");
        _shootPoint = GetNode<Position2D>("ShootPoint");   
        
    }

    public override void _Process(float delta)
    {
        Shoot();
        MoveState(delta);
    }
    public void MoveState(float delta)
    {
        Vector2 direction = Vector2.Down;
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


    public void Shoot()
    {
        var rand = new RandomNumberGenerator();
        rand.Randomize();

        if(rand.RandiRange(1,100) == 1)
            InstanceBullet();

    }
    public void InstanceBullet()
    {
        var BulletInstance = _bullet.Instance<PlayerBullet>();
        BulletInstance.Position = _shootPoint.GlobalPosition;

        GetParent().AddChild(BulletInstance);
    }

    public void DeadState()
    {
        QueueFree();
    }

    public void OnHurtboxAreaEntered(Hitbox hitbox)
    {
        // _hurtbox.ShowHitEffect();
        Health -= hitbox.damage;
        if(Health <= 0)
            DeadState();
    }

    public void OnDestroyTimerTimeout()
    {
        DeadState();
    }


}
