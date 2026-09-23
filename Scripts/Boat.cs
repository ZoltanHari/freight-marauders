using Godot;
using System;

public partial class Boat : CharacterBody2D
{
	[Export] public float Speed { get; set; } = 150f;
    [Export] public float RotationSpeed { get; set; } = 3.0f; 
	public bool Player_In_Boat = false;
	public bool isPlayerNear = false;

	public Player player;
    private Marker2D _seatMarker; 
    private Area2D _area;

	public override void _Ready()
	{
		_seatMarker = GetNode<Marker2D>("SeatMarker");
		player = GetNode<Player>("/root/Game/Player");
        _area = GetNode<Area2D>("InteractArea");
        _area.BodyEntered += OnBodyEntered;
		_area.BodyExited += OnBodyExited;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Player_In_Boat == true)
		{
			float rotationDirection = Input.GetAxis("steer_left", "steer_right");
			Rotation += rotationDirection * RotationSpeed * (float)delta;

			float moveDirection = Input.GetAxis("move_forward", "move_backward");
			Velocity = Transform.Y * moveDirection * Speed;

			MoveAndSlide();
		}
	}

	public override void _UnhandledInput(InputEvent @event)
    {
        if (isPlayerNear == true && @event.IsActionPressed("interact"))
        {
            EnterBoat();
        }

		if (Player_In_Boat == true &&  @event.IsActionPressed("ui_accept"))
		{
            ExitBoat();
		}
    }

	private void EnterBoat()
    {
        Player_In_Boat = true;
        
        player.SetPhysicsProcess(false); 
        player.SetProcessInput(false);

        player.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true; 

        player.GetParent().RemoveChild(player);
        _seatMarker.AddChild(player);

        player.Position = Vector2.Zero;
    }

    private void ExitBoat()
    {
        Player_In_Boat = false;

        _seatMarker.RemoveChild(player);


        GetParent().AddChild(player);

        player.GlobalPosition += new Vector2(50, 0);

        player.SetPhysicsProcess(true);
        player.SetProcessInput(true);

        player.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false; 
        
    }

    private void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			isPlayerNear = true;
		}
	}

    private void OnBodyExited(Node2D body)
    {
        if (body is Player)
        {
            isPlayerNear = false;
		}
	}
}
