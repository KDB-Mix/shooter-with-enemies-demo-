using Godot;
using System;

public partial class Credits : Button
{
	[Export] public Label label { get; set; }

	public void _on_button_down()
	{
		label.Visible = !label.Visible;
	}
}
