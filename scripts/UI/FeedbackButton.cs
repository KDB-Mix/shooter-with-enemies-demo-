using Godot;
using System;

public partial class FeedbackButton : Button
{
	// Called when the node enters the scene tree for the first time.
	private void _on_pressed()
	{
		OS.ShellOpen("https://forms.gle/YUAfPNNot1kabxnEA");
	}

}
