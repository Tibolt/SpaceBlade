using Godot;
using System;

public class PlayerBullet : KinematicBody2D
{
	[Export]
	public Vector2 Direction = Vector2.Up;
	[Export]
	public float MaxSpeed {get; set;}
	[Export]
	public float Acceleration {get; set;}
	[Export]
	public int Damage {get; set;}
	[Export]
	public bool IsPlayerBullet {get; set;}
	public Vector2 Velocity {get; set;}


	public override void _Ready()
	{
		Visible = true;
	}

	public override void _PhysicsProcess(float delta)
	{
		Destory();
		/* GlobalPosition += Direction * MaxSpeed * delta; */

		Velocity = new Vector2(Direction).Rotated(this.Rotation);
		Velocity = Velocity.Normalized() * MaxSpeed;

		Velocity = MoveAndSlide(Velocity);

		
	}

	public void Destory()
	{
		if(IsPlayerBullet)
		{
			if(Position.y <= GlobalVariables.ScreenTop)
				QueueFree();
		}
		else
		{
			if(Position.y >= GlobalVariables.ScreenBottom)
				QueueFree();
		}
		if(Position.x >= GlobalVariables.ScreenRight)
			QueueFree();
		else if(Position.x <= GlobalVariables.ScreenLeft)
			QueueFree();
	}

	public void OnTimerTimeout()
	{
		// TODO: Delete Timer
		/* QueueFree(); */
	}

	public void OnHitboxAreaEntered(Hurtbox hurtbox)
	{
		QueueFree();
	}


}
