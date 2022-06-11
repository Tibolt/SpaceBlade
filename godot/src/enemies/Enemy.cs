using Godot;
using System;

public class Enemy : KinematicBody2D
{
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

    public void OnHurtboxAreaEntered(Hitbox hitbox)
    {
        // _hurtbox.ShowHitEffect();
        GD.Print("Enemy - 1HP");
    }


}
