using Godot;
using System;

public partial class IdleTimer : Timer
{
    public override void _Ready()
    {
        // Call idle() on the parent node when this timer enters the tree or starts
        CallDeferred(nameof(CallIdleOnParent));
    }

    private void CallIdleOnParent()
    {
        // Assuming the parent is Enemy and has idle() method
        if (GetParent() is Enemy enemy)
        {
            enemy.idle();
        }
    }

    // Or you can call idle() every time the timer times out:
    public void _on_Timeout()
    {
        if (GetParent() is Enemy enemy)
        {
            enemy.idle();
        }
    }
}
