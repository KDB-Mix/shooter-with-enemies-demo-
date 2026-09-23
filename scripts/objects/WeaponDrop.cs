using Godot;
using System;
using System.Collections.Generic;

public partial class WeaponDrop : Area2D
{
	public List<Sprite2D> gunsSprites = new();
	public int gunSelected;
	public List<int> ammo = new();
	public TouchScreen touchscreen;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if ((GetTree().GetFirstNodeInGroup("touchscreen") is TouchScreen) && GetTree().GetFirstNodeInGroup("touchscreen") != null)
		{
			touchscreen = (TouchScreen)GetTree().GetFirstNodeInGroup("touchscreen");
		}
		foreach (Node node in GetChildren())
		{
			if (node is Sprite2D gunSprite) gunsSprites.Add(gunSprite);
		}
		ShowGun((int)GD.RandRange((int)0, (int)gunsSprites.Count - 1));
		ammo = new List<int> { 20, 50, 10 };
	}

	public override void _Process(double delta)
	{
		if (HasOverlappingBodies())
		{
			foreach (var body in GetOverlappingBodies())
			{
				if (body is PlayerMain)
				{
					touchscreen.Pickup.Visible = true;
				}
				else
				{
					touchscreen.Pickup.Visible = false;
				}
			}
		}
		else
		{
			touchscreen.Pickup.Visible = false;
		}
	}


	public void ShowGun(int ID)
	{
		if (ID < 0 || ID >= gunsSprites.Count) return;
		foreach (Sprite2D gun in gunsSprites)
		{
			gun.Visible = false;
		}
		gunsSprites[ID].Visible = true;
		gunSelected = ID;
	}
    public override void _Input(InputEvent @event)
    {
		if (@event.IsActionPressed("down"))
		{
			foreach (var body in GetOverlappingBodies())
{
				if (body is PlayerMain player)
				{
					if (player.Inventory.Count < 2)
					{
						player.Inventory.Add(gunSelected);
						player.GunSprite.ShowGun(player.Inventory[player.Inventory.Count - 1]);
						player.currentGun = gunSelected;
						player.gunID = player.Inventory[player.Inventory.Count - 1];
					}
					else player.Inventory[player.gunID] = gunSelected;
					player.GunSprite.ammo[gunSelected] = ammo[gunSelected];
					touchscreen.Pickup.Visible = false;
					QueueFree();
				}
}
		}
    }

}
