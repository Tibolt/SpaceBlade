using Godot;
using System;
using System.Linq;

public class Player : Ship
{
	public enum States
	{
		MOVE,
		DEAD,
	};

	public States State = States.MOVE;
	public bool IsReloading = false;
	[Export]
	public float ReloadTime = 1.5f;
	[Export]
	public int NumberOfBullets = 5;
	public int BulletsShooted {get;set;}
	[Export]
	public int WeaponType = 1;


	private PackedScene _bullet = (PackedScene)ResourceLoader.Load("res://src/player/PlayerBullet.tscn");
	private Position2D _shootPoint {get; set;}
	private Timer _reloadTimer {get;set;}
	private GlobalVariables _globalVariables {set;get;}
	public override void _Ready()
	{
		_shootPoint = GetNode<Position2D>("ShootPoint");   
		_reloadTimer = GetNode<Timer>("ReloadTimer");
		_globalVariables = GetNode<GlobalVariables>("/root/GlobalVariables");

		BulletsShooted = NumberOfBullets;
		_globalVariables.SetPlayerHealth(Health);

        InitShootPoints();

	}
	public override void _Input(InputEvent @event)
	{
		if(@event.IsActionPressed("reload"))
			Reload();
	}

	public override void _Process(float delta)
	{
		WrapEdges();

		switch(State)
		{
			case States.MOVE:
				MoveState(delta);
				break;
		}

		if(Input.IsActionJustPressed("shoot") && !IsReloading)
		{
			// NumberOfBullets = 0 -> unlimited shooting
			if(NumberOfBullets != 0 && BulletsShooted <= 0)
			{
				Reload();
			}
			else
			{
				InstanceBullet();
				--BulletsShooted;
			}
		}
        
        var canRotate = false;
        //rotate _shootPoint node
        if(canRotate)
        {
            var rotation = _shootPoint.Rotation + 40 * delta;
            _shootPoint.Rotate((float)rotation%360);
        }
	}
	public Vector2 GetDirection()
	{
		return new Vector2(Input.GetActionStrength("right") - Input.GetActionStrength("left"), 0);
	}

	public void MoveState(float delta)
	{
		Vector2 direction = GetDirection();
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

	public void DeadState()
	{
		// play destroy animation
	}

	public void InstanceBullet()
	{
		/* var inits = FireMode(WeaponType); */
		/* var length = inits[1]; */
		/*         for(float i = 1; i <= inits[2]; i+=1) */
		/*         { */
		/*             float xPos = _shootPoint.GlobalPosition.x; */
		/*             float yPos = _shootPoint.GlobalPosition.y; */

		/*             var bulletInstance = _bullet.Instance<PlayerBullet>(); */
		/*             bulletInstance.Position = new Vector2(xPos - length + i*length, yPos); */
		/*             bulletInstance.Rotation = (inits[3] * 3.14F)/180; */

		/*             GetParent().AddChild(bulletInstance); */
		/*         } */
        foreach(Node2D node in _shootPoint.GetChildren())
        {
            var bulletInstance = _bullet.Instance<PlayerBullet>();
            GetParent().AddChild(bulletInstance);
            bulletInstance.Position = node.GlobalPosition;
            bulletInstance.Rotation = node.GlobalRotation;

        }

	}
    public void InitShootPoints()
    {
        var radius = 1f;
        var startPos = -30f;
        var rotateSpeed = 3f;
        var interval = 0f;
        var spawningCount = 3;

        var step = Math.PI * 2 / spawningCount;
        bool curve = false;

        foreach(var i in Enumerable.Range(1, spawningCount))
        {
            var spawnPoint = new Node2D();
            var pos = new Vector2(startPos + i*20, 0);
            if(curve)
            {
                pos = new Vector2(radius, 0).Rotated((float)step * i);
                spawnPoint.Rotation = pos.Angle();
            }
            else if(WeaponType == 3)
            {
                // rotate first and last bullet 
                if(i == 1)
                {
                    spawnPoint.Rotation = -10 * (float)Math.PI / 180f;
                } 
                if(i == 3)
                {
                    spawnPoint.Rotation = 10 * (float)Math.PI / 180f;
                }
            }
            else if(WeaponType == 4)
            {
                // rotate first and last bullet 
                if(i == 1)
                {
                    spawnPoint.Rotation = -10 * (float)Math.PI / 180f;
                } 
                if(i == 4)
                {
                    spawnPoint.Rotation = 10 * (float)Math.PI / 180f;
                }
            }


            spawnPoint.Position = pos;

            _shootPoint.AddChild(spawnPoint);
        }
    }
	public float[] FireMode(int weapon)
	{
		int fireLoop = WeaponType;
		float yPos = _shootPoint.GlobalPosition.y;
		// for _shootPoint
		// xStartPos, length bettwen, number of shootPoints, rotation
		float[] positions = {0, 0, 0, 0};
		switch(weapon)
		{
			case 1:
				/* _shootPoint.Position = new Vector2(0, yPos); */
				positions = new float[] {0 ,0, 1, 0};
				break;
			case 2:
				/* _shootPoint.Position = new Vector2(-2, yPos); */
				positions = new float[] {-3 ,6, 2, 0};
				break;
			case 3:
				/* _shootPoint.Position = new Vector2(-5, yPos); */
				positions = new float[] {-5 ,5, 3, 5};
				break;
			case 4:
				/* _shootPoint.Position = new Vector2(-5, yPos); */
				positions = new float[] {-5 ,2.5F, 4, 2};
				break;
		}
		return positions;
	}

	public async void OnHurtboxAreaEntered(Hitbox hitbox)
	{
		_globalVariables.SetPlayerHealth(_globalVariables.GetPlayerHealth() - 1);

		this.Modulate = new Color(Color.Color8(255, 120, 120));
		await ToSignal(GetTree().CreateTimer((float)0.15), "timeout");
		this.Modulate = new Color(Color.Color8(255, 255, 255));
	}

	public void OnReloadTimerTimeout()
	{
		IsReloading = false;
		BulletsShooted = NumberOfBullets;
	}

	public void OnItemCollectorAreaEntered(Hurtbox hurtbox)
	{
		var item = hurtbox.GetParent<Item>();
		if(item != null)
			ItemEffects(item.ItemName);
	}

	public void ItemEffects(string name)
	{
		switch(name)
			{
				case "Health":
					Health++;
					GD.Print("Health: " + Health);
					break;
				case "Armor":
					break;
				case "Speed":
					Acceleration += 10;
					GD.Print("Acceleration: " + Acceleration);
					break;
				case "Bullet":
					NumberOfBullets++;
					break;
				case "Coin":
					GlobalVariables.Money += 10;
					break;
				case "Skull":
					Acceleration -= 20;
					break;
				default:
					break;
			}
	}

	public void WrapEdges()
	{
		if(Position.x > GlobalVariables.ScreenRight)
			Position = new Vector2(GlobalVariables.ScreenLeft, Position.y);
		if(Position.x < GlobalVariables.ScreenLeft)
			Position = new Vector2(GlobalVariables.ScreenRight, Position.y);
	}

	public void Reload()
	{
		_reloadTimer.Start(ReloadTime);
		IsReloading = true;
	}
}
