using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	[Export]
	public float MainSpeed { get; set; } = 500.0f;
	[Export]
	public float JumpVelocity { get; set; } = -1000.0f;
	[Export]
	public RayCast2D RightPlayerDetector { get; set; }
	[Export]
	public RayCast2D LeftPlayerDetector { get; set; }
	[Export]
	public PackedScene ProjectileScene { get; private set; }
	[Export]
	public Marker2D Projectilespawn { get; private set; }
	[Export]
	public Timer Cooldowntimer { get; private set; }
	[Export] public Timer JumpCooldown { get; private set; }
	[Export] public Timer IdleTimer { get; private set; }
	[Export] public Timer WalkTimer { get; private set; }
	[Export] public PackedScene dropScene { get; set; }


	private Node2D player;
	private Vector2 velocity;
	private Vector2 direction;
	private bool playerDetected;
	private float Speed = 200f;
	private bool canShoot = true;
	public bool jump;
	public bool jumpArea;
	private int jumps;
	private bool canJump = true;
	private bool followPlayer;

	public override void _Ready()
	{
		player = (Node2D)GetTree().GetFirstNodeInGroup("player");
	}


	public override void _PhysicsProcess(double delta)
	{
		if (LeftPlayerDetector.GetCollider() is PlayerMain)
		{
			playerDetected = true;
			direction.X = -1;
			HandlePlayerDetected();
		}
		else if (RightPlayerDetector.GetCollider() is PlayerMain)
		{
			playerDetected = true;
			direction.X = 1;
			HandlePlayerDetected();
		}
		else
		{
			playerDetected = false;
			Speed = MainSpeed;
		}
		velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}
		else jumps = 2;
		Movement();
		Velocity = velocity;
		MoveAndSlide();
	}

	private void HandlePlayerDetected()
	{
		if (IsOnFloor()) Speed = 0;
		if (canShoot)
		{
			ProjectileEnemy projectile = (ProjectileEnemy)ProjectileScene.Instantiate();
			AddSibling(projectile);
			projectile.GlobalPosition = Projectilespawn.GlobalPosition;
			projectile.direction.X = direction.X;
			Cooldowntimer.Start();
			canShoot = false;
		}
	}
	public void idle()
	{
		IdleTimer.WaitTime = GD.RandRange(1f, 5f);
		IdleTimer.Start();
		direction.X = 0;
	}

	public void Movement()
	{
		if (followPlayer) direction = (player.GlobalPosition - GlobalPosition).Normalized();

		// Handle Jump.


		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.

		velocity.X = Speed * direction.X;
		if (direction.X != 0 && (jumps > 0 || IsOnFloor()) && (jump || (jumpArea && ((followPlayer && (player.GlobalPosition.Y - GlobalPosition.Y) < 0) || GD.Randf() < .5))))
		{
			velocity.Y = JumpVelocity;
			jumps -= 1;
			canJump = false;
			JumpCooldown.Start();
		}
	}
	public void _on_cooldown_timeout()
	{
		canShoot = true;
	}
	public void _on_jump_detector_body_entered(Node2D body)
	{
		if (body is TileMapLayer) jump = true;
	}
	public void _on_jump_detector_body_exited(Node2D body)
	{
		if (body is TileMapLayer) jump = false;
	}
	public void _on_jump_cooldown_timeout()
	{
		canJump = true;
	}
	public void _on_follow_player_area_body_entered(Node2D body)
	{
		if (body is PlayerMain) followPlayer = true;
		Speed = MainSpeed;
		IdleTimer.Stop();
		WalkTimer.Stop();
	}
	public void _on_exit_follow_area_body_exited(Node2D body)
	{
		if (body is PlayerMain) followPlayer = false;
		direction = Vector2.Zero;
		Speed = 200f;
		idle();
	}
	public void _on_idle_timer_timeout()
	{
		if (followPlayer) return;
		direction.X = GD.Randf() < 0.5f ? -1 : 1;
		WalkTimer.WaitTime = GD.RandRange(1f, 5f);
		WalkTimer.Start();
	}
	public void _on_walk_timer_timeout()
	{
		if (followPlayer) return;
		idle();
	}
}
