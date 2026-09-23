using Godot;
using System;

public partial class HitArea : Area2D
{
    private RandomNumberGenerator rng = new RandomNumberGenerator();
    public int health = 3;
    public override void _Ready()
    {
        rng.Randomize();
    }

    public override void _Process(double delta)
    {
        if (health <= 0) CallDeferred(nameof(delete));
    }
    public void delete()
    {
        float chance = rng.Randf();
        if (chance > 0.5 && GetParent() is Enemy enemy)
        {
            Node instantiatedScene = enemy.dropScene.Instantiate();
            if (instantiatedScene is Drop drop)
            {
                enemy.AddSibling(drop);
                drop.GlobalPosition = enemy.GlobalPosition + new Vector2(0, -10);
            }
        }
        CallDeferred(nameof(deleteParent));
    }
    public void deleteParent()
    {
        GetParent().QueueFree();
    }
}
