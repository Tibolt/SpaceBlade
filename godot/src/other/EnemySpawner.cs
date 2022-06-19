using Godot;
using System;

public class EnemySpawner : Node2D
{
    [Export]
    public PackedScene enemy {get;set;}
    RandomNumberGenerator rand = new RandomNumberGenerator();
    private Timer _timer {get;set;}
    private Camera2D _shape {get;set;}

    public override void _Ready()
    {
        rand.Randomize();
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
            var enemyInstance = enemy.Instance<Enemy>();
            const float border = 100;

            // var rect = GetViewportRect();
            // var positionLeft = rect.Position.x + border;
            // var positionRight = rect.Position.x + rect.Size.x - border;
            // // var positionY = rect.Position.y;
            var positionY = GlobalVariables.ScreenTop;
            rand.Randomize();
            var position = rand.RandfRange(GlobalVariables.ScreenLeft + border, GlobalVariables.ScreenRight - border);
            // GD.Print("pos: " + position + "y: " + positionY);

            enemyInstance.Position = new Vector2(position, positionY);

            GetParent().AddChild(enemyInstance);
        }
        catch (System.Exception)
        {
            GD.Print("SpawnEnemy not working");
            throw;
        }
    }

    public void SetTimer()
    {
        rand.Randomize();

        var time = rand.RandfRange(1,4);
        // var1 + (randf() - var2) * var3
        _timer.WaitTime = time;
    }

    public void OnTimerTimeout()
    {
        SpawnEnemy();
        SetTimer();
    }
}
