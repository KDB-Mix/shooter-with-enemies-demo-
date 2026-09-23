using Godot;
using System;

public partial class HitAreaPlayer : Area2D
{
    public int health = 10;
    private bool canTakeDamage = true;

    [Export] public Timer DamageCooldown;
    private CollisionShape2D collisionShape;

    public override void _Ready()
    {
        // Cache the collision shape for easy access
        collisionShape = GetChild<CollisionShape2D>(0);
    }

    public override void _Process(double delta)
    {
        if (health <= 0)
        {
            GetTree().ReloadCurrentScene();
            // GetParent().QueueFree();
        }
    }

    public void TakeDamage(int amount)
    {
        if (!canTakeDamage)
            return;

        health -= amount;
        canTakeDamage = false;
        CallDeferred("disableCollision");
        DamageCooldown.Start();
    }

    public void _on_damage_cooldown_timeout()
    {
        canTakeDamage = true;
        collisionShape.Disabled = false; // Re-enable collision
    }
    public void disableCollision()
    {
        collisionShape.Disabled = true;
    }
}
