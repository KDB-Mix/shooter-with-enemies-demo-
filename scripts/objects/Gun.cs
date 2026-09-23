using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public partial class Gun : Node2D
{
	public Marker2D projectileSpawn;
	public Array<Sprite2D> gunsSprites = new();
	public List<int> ammo = new();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		foreach (Node node in GetChildren())
		{
			if (node is Sprite2D gunSprite) gunsSprites.Add(gunSprite);
		}
		ammo = new List<int> { 20, 50, 10 };
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void ShowGun(int ID)
	{
		if (ID < 0 || ID >= gunsSprites.Count)
			GD.PushWarning($"[ShowGun] Gun Not Found: {ID}");
		foreach (Sprite2D gun in gunsSprites)
		{
			gun.Visible = false;
		}
		gunsSprites[ID].Visible = true;
		projectileSpawn = gunsSprites[ID].GetChild<Marker2D>(0);
	}
}
