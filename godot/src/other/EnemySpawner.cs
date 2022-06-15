using Godot;
using System;

public class EnemySpawner : Node2D
{
    [Export]
    public PackedScene enemy {get;set;}
    private Timer _timer {get;set;}
    // private CollisionShape2D _shape {get;set;}
    private Camera2D _shape {get;set;}

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        // _shape = GetNode<CollisionShape2D>("Area2D/CollisionShape2D");
        _shape = GetNode<Camera2D>("Camera2D");
    }

    public override void _Process(float delta)
    {

    }

    public void SpawnEnemy()
    {
        try
        {
            var enemyInstance = enemy.Instance<Enemy>();

            RandomNumberGenerator rand = new RandomNumberGenerator();
            rand.Randomize();

            var positionLeft = _shape.LimitLeft;
            var positionRight = _shape.LimitRight;
            var positionY = _shape.GlobalPosition.y;
            var position = rand.RandiRange(positionLeft, positionRight);

            enemyInstance.Position = new Vector2(position, positionY);

            GetParent().AddChild(enemyInstance);
        }
        catch (System.Exception)
        {
            GD.Print("Now working");
            throw;
        }
    }

    public void SetTimer()
    {
        RandomNumberGenerator rand = new RandomNumberGenerator();
        rand.Randomize();

        var time = rand.RandfRange(2,6);
        _timer.WaitTime = time;
    }

    public void OnTimerTimeout()
    {
        SpawnEnemy();
        SetTimer();
    }
}
