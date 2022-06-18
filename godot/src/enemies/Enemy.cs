using Godot;
using System;

public class Enemy : Ship
{
    [Export]
    public bool CanStrafe = false;
    public Vector2 directionX = Vector2.Zero;
    [Export]
    public float ReloadTime = 1;
    public bool IsReloading = false;
    private Sprite _sprite {get;set;}
    private CollisionShape2D _collisionShape {get;set;}
    private Hurtbox _hurtbox = null;
    private Position2D _shootPoint {get; set;}
    private Timer _reloadTimer {get;set;}
    private PackedScene _bullet = (PackedScene)ResourceLoader.Load("res://src/enemies/EnemyBullet.tscn");
    [Export]
    public PackedScene Item {get;set;}
    public override void _Ready()
    {
        _sprite = GetNode<Sprite>("Sprite");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        _hurtbox = GetNode<Hurtbox>("Hurtbox");
        _shootPoint = GetNode<Position2D>("ShootPoint");   
        _reloadTimer = GetNode<Timer>("ReloadTimer");
         
        var rand = new RandomNumberGenerator();
        rand.Randomize();
        if(rand.RandiRange(1,2) == 1)
            directionX = Vector2.Left;
        else
            directionX = Vector2.Right;
    }

    public override void _Process(float delta)
    {
        Shoot();
        MoveState(delta);
        if(CanStrafe)
            Strafe(delta);
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
    public void Strafe(float delta)
    {
        // TODO: change left and right to screen size
        var left = 300;
        var right = 700;
        directionX = directionX.Normalized();

        if(GlobalPosition.x <= left)
            directionX = Vector2.Right;
        else if(GlobalPosition.x >= right)
            directionX = Vector2.Left;

        if(directionX != Vector2.Zero)
            Velocity = Velocity.MoveToward(directionX * MaxSpeed, Acceleration * delta);

        Velocity = MoveAndSlide(Velocity);
    }

    public void RandomStrafe()
    {
        var rand = new RandomNumberGenerator();
        rand.Randomize();
        

        if(rand.RandiRange(1,2) == 1)
        {
            directionX = Vector2.Left;
        }
        else
        {
            directionX = Vector2.Right;
        }
    }


    public void Shoot()
    {
        var rand = new RandomNumberGenerator();
        rand.Randomize();

        if(!IsReloading && rand.RandiRange(1,100) == 1)
        {
            InstanceBullet();
            IsReloading = true;
            _reloadTimer.Start();
        }
    }
    public void InstanceBullet()
    {
        var BulletInstance = _bullet.Instance<PlayerBullet>();
        BulletInstance.Position = _shootPoint.GlobalPosition;

        GetParent().AddChild(BulletInstance);
    }

    public void InstaceItem()
    {
        var drop = Item.Instance<Item>();
        drop.Position = GlobalPosition;

        GetParent().AddChild(drop);
    }

    public void DeadState()
    { 
        var rand = new RandomNumberGenerator();
        rand.Randomize();

        if(rand.RandiRange(1,10) == 1)
            InstaceItem();

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
        QueueFree();
    }

    public void OnReloadTimerTimeout()
    {
        IsReloading = false;
        RandomStrafe();
    }


}
