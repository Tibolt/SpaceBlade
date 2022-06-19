using Godot;
using System;

public class PauseScreen : Control
{
    public bool IsPaused {set;get;}

    private ColorRect _canvas {set;get;}
    private Label _scoreLabel {set;get;}
    public override void _Ready()
    {
        _canvas = GetNode<ColorRect>("ColorRect");
        _scoreLabel = GetNode<Label>("ScoreLabel");
    }

    public override void _Process(float delta)
    {
        UpdateInterface();
    }
    public override void _UnhandledInput(InputEvent @event)
    {
        if(@event.IsActionPressed("pause"))
        {
            // IsPaused = !IsPaused;
            SetPause(!IsPaused);
            GetTree().SetInputAsHandled();
        }
    }

    public void SetPause(bool value)
    {
        IsPaused = value;
        GetTree().Paused = value;
        _canvas.Visible = value;

    }

    public void UpdateInterface()
    {
        // update UI, score, health
        _scoreLabel.Text = GlobalVariables.Score.ToString();
    }

        


}
