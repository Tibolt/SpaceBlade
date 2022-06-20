using Godot;
using System;

public class EnemySpawner : Node2D
{
    [Export]
    public PackedScene Enemy {get;set;}
    [Export] public Godot.Collections.Array<PackedScene> EnemyList = new Godot.Collections.Array<PackedScene>();
    private RandomNumberGenerator _rand = new RandomNumberGenerator();
    private Timer _timer {get;set;}
    private Camera2D _shape {get;set;}

    public override void _Ready()
    {
        _rand.Randomize();
        _timer = GetNode<Timer>("Timer");
        _shape = GetNode<Camera2D>("Camera2D");
    }

    public override void _Process(float delta)
    {

    }

    public void SpawnEnemy()
    {
        try
        {
            // var enemyInstance = Enemy.Instance<Enemy>();
            var enemyInstance = ChooseEnemy().Instance<Enemy>();
            const float border = 100;

            var positionY = GlobalVariables.ScreenTop;
            _rand.Randomize();
            var position = _rand.RandfRange(GlobalVariables.ScreenLeft + border, GlobalVariables.ScreenRight - border);

            enemyInstance.Position = new Vector2(position, positionY);
            GetParent().AddChild(enemyInstance);
        }
        catch (System.Exception)
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

        var time = _rand.RandfRange(1,4);
        _timer.WaitTime = time;
    }

    public void OnTimerTimeout()
    {
        SpawnEnemy();
        SetTimer();
    }
}
