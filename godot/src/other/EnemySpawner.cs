using Godot;
using System;

public class EnemySpawner : Node2D
{
    [Export]
    public PackedScene Enemy {get;set;}
    [Export] 
    public Godot.Collections.Array<PackedScene> EnemyList = new Godot.Collections.Array<PackedScene>();
    [Export]
    public float MinTimeSpawn {get;set;}
    [Export]
    public float MaxTimeSpawn {get;set;}
    [Export]
    public float TimeToShortenSpawn {get;set;}
    private RandomNumberGenerator _rand = new RandomNumberGenerator();
    private Timer _timer {get;set;}
    private Camera2D _shape {get;set;}
    private bool _spawnerUpdated = false;
    [Signal]
    public delegate void NextScore();

    public override void _Ready()
    {
        _rand.Randomize();
        _timer = GetNode<Timer>("Timer");
        _shape = GetNode<Camera2D>("Camera2D");

        Connect("NextScore", this, "UpdateSpawner");
    }

    public override void _Process(float delta)
    {
        if(GlobalVariables.Score % 10 > 0)
        {
            EmitSignal("NextScore");
        }
        else
        {
            _spawnerUpdated = false;
        }
    }

    public void SpawnEnemy()
    {
        try
        {
            // var enemyInstance = Enemy.Instance<Enemy>();
            var enemyInstance = ChooseEnemy().Instance<Enemy>();
            const float border = 100;
            const float topBorder = 4;

            _rand.Randomize();
            var position = _rand.RandfRange(GlobalVariables.ScreenLeft + border, GlobalVariables.ScreenRight - border);
            var positionY = GlobalVariables.ScreenTop + topBorder;

            enemyInstance.Position = new Vector2(position, positionY);
            GetParent().AddChild(enemyInstance);
        }
        catch
        {
            GD.Print("SpawnEnemy not working");
        }
    }

    public PackedScene ChooseEnemy()
    {
        _rand.Randomize();

        var randomEnemy = EnemyList[_rand.RandiRange(0, EnemyList.Count-1)];
        return randomEnemy;
    }

    public void SetTimer()
    {
        _rand.Randomize();

        var time = _rand.RandfRange(MinTimeSpawn,MaxTimeSpawn);
        _timer.WaitTime = time;
    }

    public void OnTimerTimeout()
    {
        SpawnEnemy();
        SetTimer();
    }

    public void UpdateSpawner()
    {
        if(MaxTimeSpawn > MinTimeSpawn + 0.5 && !_spawnerUpdated)
        {
            MaxTimeSpawn -= TimeToShortenSpawn;
            _spawnerUpdated = true;
        }
    }
}
