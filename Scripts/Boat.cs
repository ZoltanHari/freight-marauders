using Godot;
using System;

public partial class Boat : CharacterBody2D
{
	[Export] public float Speed { get; set; } = 150f;
    [Export] public float RotationSpeed { get; set; } = 3.0f; 
	public bool Boat_Can_Move = false;

	public override void _Ready()
	{
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Boat_Can_Move == true)
		{
			float rotationDirection = Input.GetAxis("steer_left", "steer_right");
			Rotation += rotationDirection * RotationSpeed * (float)delta;

			float moveDirection = Input.GetAxis("move_forward", "move_backward");
			Velocity = Transform.Y * moveDirection * Speed;

			MoveAndSlide();
		}
	}
}
