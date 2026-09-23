using Godot;
using System;
using System.Collections.Generic;

public class GunInfo
{
    public string Name;
    public int Ammo;

    public GunInfo(string name, int ammo)
    {
        Name = name;
        Ammo = ammo;
    }
}

public partial class PlayerMain : CharacterBody2D
{
	//Creating
	private bool buffer;
	private int extraJumps;
	private bool cyote;
	private Vector2 velocity;
	private Marker2D spawnpoint;
	private Vector2 direction = new Vector2(1, 0);
	private Vector2 lastDirection = new Vector2(1, 0);
	private bool canShoot = true;
	public int gunID;
    public int currentGun;
    private Vector2 scale = new Vector2(1, 1);
	public List<int> Inventory = new();

	//Exported
	[Export]
	public float Speed { get; set; } = 750.0f;
	[Export]
	public float JumpVelocity { get; set; } = -750.0f;
	[Export]
	public Timer BufferTimer { get; set; }
	[Export]
	public Timer CyoteTimer { get; set; }
	[Export]
	public PackedScene Projectile { get; set; }
	[Export]
	public Marker2D Projectilespawn { get; set; }
	[Export]
	public Timer CooldownTimer { get; set; }
	[Export] public Label label { get; set; }
	[Export] public HitAreaPlayer hitAreaPlayer { get; set; }
	[Export] public Gun GunSprite { get; set; }

	public override void _Ready()
	{
		spawnpoint = (Marker2D)GetTree().GetFirstNodeInGroup("spawn");
		if (spawnpoint != null) GlobalPosition = spawnpoint.GlobalPosition;
		GunSprite.ShowGun(0);
		Inventory = new List<int> {0};
	}


	public override void _PhysicsProcess(double delta)
	{
		label.Text = "" + hitAreaPlayer.health;

		if (Input.IsActionPressed("shoot") && canShoot)
		{
			switch (currentGun)
			{
				case 1:
					AutoRifle();
					break;
			}
		}
		if (Input.IsActionJustPressed("shoot") && canShoot)
		{
			switch (currentGun)
			{
				case 0:
					Pistol();
					break;
				case 2:
					ShotGun();
					break;
			}
		}
		if (IsOnFloor())
		{
			extraJumps = 1;
			CyoteTimer.Stop();
			CyoteTimer.Start();
			cyote = true;
		}
		velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump"))
		{
			if (IsOnFloor() || cyote)
			{
				velocity.Y = JumpVelocity;
				buffer = false;
				cyote = false;
			}
			else if (extraJumps > 0)
			{
				velocity.Y = JumpVelocity;
				buffer = false;
				extraJumps -= 1;
			}
			else
			{
				buffer = true;
				BufferTimer.Start();
			}
		}
		if (buffer && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
			buffer = false;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.

		HandleMovement();
		if ((lastDirection.X != direction.X) && (direction.X != 0))
		{
			if (direction.X == 1)
			{
				if (scale.X == -1) Scale = new Vector2(-1, Scale.Y);
				scale.X = 1;

			}
			else if (direction.X == -1)
			{
				if (scale.X == 1) Scale = new Vector2(-1, Scale.Y);
				scale.X = -1;

			}
		}
		Velocity = velocity;
		MoveAndSlide();
		if (direction.X != 0) lastDirection = direction;
	}

	private void ShotGun()
	{
		if (GunSprite.ammo[2] > 0)
		{
			for (int i = 0; i < 5; i++)
			{
				ShootProjectile(1, -30 + 15 * i);
			}
			canShoot = false;
			CooldownTimer.WaitTime = .5f;
			CooldownTimer.Start();
			GunSprite.ammo[2] -= 1;
		}
	}

	public void _on_buffer_timer_timeout()
	{
		buffer = false;
	}
	public void _on_cyote_timer_timeout()
	{
		cyote = false;
	}

	public void HandleMovement()
	{
		direction = Input.GetVector("left", "right", "up", "down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed / 7);
		}
	}

	public void ShootProjectile(int damage, float deg)
	{
		Projectile projectile = (Projectile)Projectile.Instantiate();
		AddSibling(projectile);
		projectile.damage = damage;
		projectile.direction = DegToDirection(deg);
		projectile.GlobalPosition = GunSprite.projectileSpawn.GlobalPosition;
		projectile.direction.X = lastDirection.X;
	}
	public void _on_cooldown_timeout()
	{
		canShoot = true;
	}
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("changeWeapon"))
		{
			gunID = (gunID + 1) % Inventory.Count;
			currentGun = Inventory[gunID];
			GunSprite.ShowGun(currentGun);
		}
	}
	public void Pistol()
	{
		if (GunSprite.ammo[0] > 0)
		{
			ShootProjectile(2, 0);
			canShoot = false;
			CooldownTimer.WaitTime = .3f;
			CooldownTimer.Start();
			GunSprite.ammo[0] -= 1;
		}
	}
	public void AutoRifle()
	{
		if (GunSprite.ammo[1] > 0)
		{
			ShootProjectile(1, 0);
			canShoot = false;
			CooldownTimer.WaitTime = .1f;
			CooldownTimer.Start();
			GunSprite.ammo[1] -= 1;
		}
	}
	public static Vector2 DegToDirection(float degrees)
	{
		float radians = Mathf.DegToRad(degrees);
		return new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).Normalized();
	}
}
