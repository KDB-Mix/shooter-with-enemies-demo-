using Godot;
using System;
using System.Collections;

public partial class Projectile : Area2D
{
	public Vector2 direction;
    private HitArea hitArea;
    public int damage;


    [Export]
	public float Speed { get; private set; } = 1000;


	public override void _Process(double delta)
	{
		if (direction != Vector2.Zero) direction = direction.Normalized();

		GlobalPosition += direction * Speed * (float)delta;
	}

	public void _on_area_entered(Area2D area)
	{
		if (area is HitArea)
		{
			hitArea = (HitArea)area;
			hitArea.health -= damage;
			QueueFree();
		}
	}
	public void _on_body_entered(Node2D body)
	{
		if (body is TileMapLayer) QueueFree();
	}
}