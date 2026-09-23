using Godot;
using System;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

public partial class ProjectileEnemy : Area2D
{
    public Vector2 direction;
    [Export] public float Speed { get; private set; } = 1000;

    public override void _Process(double delta)
    {
        if (direction != Vector2.Zero)
            direction = direction.Normalized();

        GlobalPosition += direction * Speed * (float)delta;
    }

    public void _on_area_entered(Area2D area)
    {
        if (area is HitAreaPlayer player)
        {
            player.TakeDamage(1);
            CallDeferred("selfDelete");
        }
    }

    public void _on_body_entered(Node2D body)
    {
        if (body is TileMapLayer)
            QueueFree();
    }
    public void selfDelete()
    {
        QueueFree();
    }
}
