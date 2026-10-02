using Godot;
using System;

public partial class BoatText : Label
{
	[Export] public float VerticalOffset { get; set; } = 35.0f;
	public Label boatLabel;

	public override void _Ready()
	{
		boatLabel = GetNode<Label>("/root/Game/Boat/Boat Text");
	}

	public override void _Process(double delta)
	{
		boatLabel.GlobalPosition = GlobalPosition + new Vector2(0, VerticalOffset);
	}
}
