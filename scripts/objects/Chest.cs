using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

public partial class Chest : Area2D
{
	private Node instantiatedScene;
    public WeaponDrop weaponScene;

    [Export] public PackedScene WeaponScene { get; set; }
	[Export] public AnimatedSprite2D animation { get; set; }
	[Export] public CollisionShape2D collision { get; set; }
	public void _on_body_entered(Node2D body)
	{
		if (body is PlayerMain)
		{
			instantiatedScene = WeaponScene.Instantiate();
			if (instantiatedScene is WeaponDrop weaponDrop)
			{
				weaponScene = weaponDrop;
				animation.Play("Open");
				CallDeferred(nameof(SpawnWeapon));
				weaponScene.GlobalPosition = GlobalPosition + new Vector2(GD.Randf() * ((GD.Randf() > .5) ? -30 : 30), -10);
				collision.QueueFree();
			}
		}
	}
	private void SpawnWeapon()
	{
		AddSibling(weaponScene);
	}

}
