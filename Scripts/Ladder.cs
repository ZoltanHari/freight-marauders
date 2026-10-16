using Godot;
using System;

public partial class Ladder : Area2D
{
	public Player player;
	private bool _isPlayerNear = false;
	private bool _inShip = false;
	private Marker2D _enterShip; 
	private Marker2D _exitShip;
	
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		player = GetNode<Player>("/root/Game/Player");
		_enterShip = GetNode<Marker2D>("../EnterShip");
		_exitShip = GetNode<Marker2D>("../ExitShip");
	}

	public override void _PhysicsProcess(double delta)
{
    if (Input.IsActionJustPressed("interact") && _isPlayerNear && !_inShip)
    {
        player.GetParent().RemoveChild(player);
        _enterShip.AddChild(player);
        player.Position = Vector2.Zero;

        _isPlayerNear = false;
        _inShip = true;
    }
    else if (Input.IsActionJustPressed("interact") && _isPlayerNear && _inShip)
    {
        player.GetParent().RemoveChild(player); 
        GetParent().AddChild(player);
        player.GlobalPosition = _exitShip.GlobalPosition;

        _isPlayerNear = false;
        _inShip = false;
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
