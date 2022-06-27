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
    [Export]
    public PackedScene ItemScene {get;set;}
    [Export]
    public int DropRate {get;set;}
    public string LastDrop = "";
    public bool DontDrop {get;set;}
    private RandomNumberGenerator _rand = new RandomNumberGenerator();
    private Sprite _sprite {get;set;}
    private CollisionShape2D _collisionShape {get;set;}
    private Hurtbox _hurtbox = null;
    private Position2D _shootPoint {get; set;}
    private Timer _reloadTimer {get;set;}
    private PackedScene _bullet = (PackedScene)ResourceLoader.Load("res://src/enemies/EnemyBullet.tscn");

    public override void _Ready()
    {
        _sprite = GetNode<Sprite>("Sprite");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        _hurtbox = GetNode<Hurtbox>("Hurtbox");
        _shootPoint = GetNode<Position2D>("ShootPoint");   
        _reloadTimer = GetNode<Timer>("ReloadTimer");
         
        _rand.Randomize();
        if(_rand.Randi() % 2 == 0)
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
        const float border = 4;
        var left = GlobalVariables.ScreenLeft;
        var right = GlobalVariables.ScreenRight;
        directionX = directionX.Normalized();

        if(Position.x <= left + border)
            directionX = Vector2.Right;
        else if(Position.x >= right - border)
            directionX = Vector2.Left;

        if(directionX != Vector2.Zero)
            Velocity = Velocity.MoveToward(directionX * MaxSpeed, Acceleration * delta);

        Velocity = MoveAndSlide(Velocity);
    }

    public void RandomStrafe()
    {
        _rand.Randomize();

        if(_rand.Randi() % 2 == 0)
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
        _rand.Randomize();

        if(!IsReloading && _rand.Randi() % 200 == 0)
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
        // var item = Item.SetItem("Skull");
        var drop = ItemScene.Instance<Item>();
        // drop.ItemName = "Skull";
        drop.Position = GlobalPosition;

        GetParent().AddChild(drop);
        drop.SetItemName(RandomItem());
        drop.CallDeferred("SetCollisionShape",5);
    }

    public string RandomItem()
    {
        _rand.Randomize();
        var items = GlobalVariables.ItemNames;
        var itemsWeight = GlobalVariables.ItemsDict;
        
        var chance = _rand.Randf();
        GD.Print(chance);
        string dr = null;
        dr = itemsWeight.ContainsRarity(chance);

        if(dr != null)
            return LastDrop = dr;
        GD.Print("Retrun rand drop");
        var drop = items[_rand.RandiRange(0, items.Count-1)];
        return drop;
    }

    public void DeadState()
    { 
        GlobalVariables.Score += 1;

        if(_rand.Randi() % DropRate == 0)
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
        if(CanStrafe)
            RandomStrafe();
    }


}
