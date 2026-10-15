using Godot;
using System;

public partial class Ladder : Area2D
{
	public Player player;
	private bool _isPlayerNear = false;
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		player = GetNode<Player>("/root/Game/Player");
	}

	public override void _PhysicsProcess(double delta)
	{
		if  (Input.IsActionJustPressed("interact") && _isPlayerNear)
		{
			Vector2 exitPosition = GlobalPosition + new Vector2(0, 5); 
    		player.GlobalPosition = exitPosition;
			_isPlayerNear = false;
		}
	}

	private void OnBodyEntered(Node body)
    {
		if (body is Player)
		{
			_isPlayerNear = true;
		}

	}
}
