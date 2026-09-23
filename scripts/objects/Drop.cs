using Godot;
using System;
using System.Collections.Generic;

public partial class Drop : Area2D
{
    private List<Sprite2D> itemsSprites = new();
    private int itemSelected;

    public override void _Ready()
    {
        foreach (Node node in GetChildren())
        {
            if (node is Sprite2D itemSprite) itemsSprites.Add(itemSprite);
        }
        ShowItem((int)GD.RandRange((int)0, (int)itemsSprites.Count - 1));
    }

    public void ShowItem(int ID)
    {
        if (ID < 0 || ID >= itemsSprites.Count) return;
        foreach (Sprite2D gun in itemsSprites)
        {
            gun.Visible = false;
        }
        itemsSprites[ID].Visible = true;
        itemSelected = ID;
    }

    public void _on_body_entered(Node2D body)
    {
        if (body is PlayerMain player)
        {
            switch (itemSelected)
            {
                case 0:
                    player.hitAreaPlayer.health = 10;
                    break;
                case 1:
                    player.GunSprite.ammo[player.currentGun] += 20;
                    break;
            }
            CallDeferred(nameof(delete));
        }
    }
    public void delete()
    {
        QueueFree();
    }
}
