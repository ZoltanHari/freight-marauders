using Godot;
using System;

public partial class InteractArea : Area2D
{

	public Boat boat;
	public Player player;
	private bool _isPlayerInside = false;

	public override void _Ready()
	{
		boat = GetParent<Boat>();
		player = GetNode<Player>("/root/Game/Player");
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	 public override void _UnhandledInput(InputEvent @event)
    {
        if (_isPlayerInside == true && @event.IsActionPressed("interact"))
        {
            if (boat != null && player != null)
            {
                boat.Boat_Can_Move = true;
                player.Player_Can_Move = false;
                GD.Print("Player entered boat");
            }
        }

		if (_isPlayerInside == false &&  @event.IsActionPressed("interact"))
		{
			boat.Boat_Can_Move = false;
        	player.Player_Can_Move = true;
            GD.Print("Player exited boat");
		}
    }

	private void OnBodyEntered(Node body)
	{
		GD.Print($"A 2D body entered: {body.Name}");

		if (body is Player && boat != null)
		{
			_isPlayerInside = true;
			GD.Print("Player Enter Area");
		}
	}

	private void OnBodyExited(Node2D body)
    {
        if (body is Player)
        {
            _isPlayerInside = false;
			GD.Print("Player Exit Area");
		}
	}
}

