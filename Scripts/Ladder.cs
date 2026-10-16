using Godot;
using System;

public partial class Ladder : Area2D
{
	public Player player;
	private Area2D _area;
	private bool _isPlayerNear = false;
	private bool _inShip = false;
	
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		player = GetNode<Player>("/root/Game/Player");
		_area = GetNode<Area2D>("/root/CargoShip/InShip");
		_area.BodyEntered += OnBodyEntered;
	}

	public override void _PhysicsProcess(double delta)
	{
		if  (Input.IsActionJustPressed("interact") && _isPlayerNear && !_inShip)
		{
			Vector2 exitPosition = GlobalPosition + new Vector2(0, 5); 
    		player.GlobalPosition = exitPosition;
			_isPlayerNear = false;
		}
		
		if  (Input.IsActionJustPressed("interact") && _isPlayerNear && _inShip)
		{
			Vector2 exitPosition = GlobalPosition + new Vector2(0, -5); 
			player.GlobalPosition = exitPosition;
			_isPlayerNear = false;
		}
	}

	private void OnBodyEntered(Node body)
    {
		if (body is Player && !_inShip)
		{
			_isPlayerNear = true;
			_inShip = true;
		}

		if (body is Player && _inShip)
		{
			_inShip = false;
			_isPlayerNear = false;
		}
	}
}
