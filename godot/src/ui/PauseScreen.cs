using Godot;
using System;

public class PauseScreen : Control
{
	public bool IsPaused {set;get;}

	private ColorRect _canvas {set;get;}
	private Label _scoreLabel {set;get;}
	private Label _moneyLabel {set;get;}
	private Label _healthLabel {set;get;}
	private GlobalVariables _globalVariables {set;get;}

	public override void _Ready()
	{
		_canvas = GetNode<ColorRect>("ColorRect");
		_scoreLabel = GetNode<Label>("ScoreLabel");
		_moneyLabel = GetNode<Label>("MoneyLabel");
		_healthLabel = GetNode<Label>("HealthLabel");
		_globalVariables = GetNode<GlobalVariables>("/root/GlobalVariables");


		_globalVariables.Connect("NoHealth", this, "PauseGame");
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
			if(!GlobalVariables.EndGame)
				PauseGame();
		}
	}

	public void SetPause(bool value)
	{
		IsPaused = value;
		GlobalVariables.IsPaused = value;
		GetTree().Paused = value;
		_canvas.Visible = value;

	}

	public void UpdateInterface()
	{
		// update UI, score, health
		_scoreLabel.Text = GlobalVariables.Score.ToString();
		_moneyLabel.Text = GlobalVariables.Money.ToString();
		_healthLabel.Text = "Health " + _globalVariables.GetPlayerHealth().ToString();
	}

	public void PauseGame()
	{
		SetPause(!IsPaused);
		GetTree().SetInputAsHandled();
	}

	public void OnRetryPressed()
	{
		GetTree().ReloadCurrentScene();
		PauseGame();
		_globalVariables.RestartGame();
	}

	public void OnExitPressed()
	{
		GetTree().Quit();
	}
}
