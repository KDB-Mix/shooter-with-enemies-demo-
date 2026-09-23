using Godot;
using System;

public partial class Jump : Area2D
{
	private Enemy enemy;
	public void _on_body_entered(Node2D body)
	{
		if (body is Enemy)
		{
			enemy = (Enemy)body;
			enemy.jumpArea = true;
		}
	}
	public void _on_body_exited(Node2D body)
	{
		if (body is Enemy)
		{
			enemy = (Enemy)body;
			enemy.jumpArea = false;
		}
	}
}
