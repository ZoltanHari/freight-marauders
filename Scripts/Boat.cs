using Godot;
using System;

public partial class Boat : CharacterBody2D
{
	[Export] public float Speed { get; set; } = 150f;
    [Export] public float RotationSpeed { get; set; } = 3.0f; 
    [Export] public float VerticalOffset { get; set; } = 35.0f;
    [Export] public float HorizontalOffset { get; set; } = -70.0f;
	public bool Player_In_Boat = false;
	private bool _isPlayerNear = false;
    private bool _canToggle = true;

	public Player player;
    private Marker2D _seatMarker; 
    private Area2D _area;
    public Label boatLabel;

	public override void _Ready()
	{
		_seatMarker = GetNode<Marker2D>("SeatMarker");
		player = GetNode<Player>("/root/Game/Player");
        _area = GetNode<Area2D>("InteractArea");
        boatLabel = GetNode<Label>("/root/Game/Boat/Boat Text");
        _area.BodyEntered += OnBodyEntered;
		_area.BodyExited += OnBodyExited;
	}
	public override void _PhysicsProcess(double delta)
	{
        if (Input.IsActionJustPressed("interact") && _canToggle)
        {
            ToggleVehicleState();
        }

        if (Player_In_Boat)
        {
            float rotationDirection = Input.GetAxis("steer_left", "steer_right");
			Rotation += rotationDirection * RotationSpeed * (float)delta;

			float moveDirection = Input.GetAxis("move_forward", "move_backward");
			Velocity = Transform.Y * moveDirection * Speed;

			MoveAndSlide();
        }

        if (!Player_In_Boat && _isPlayerNear)
        {
            StartInputCooldown();
            boatLabel.Visible = true; 
        }

        else
        {
            boatLabel.Visible = false;
        }

        boatLabel.GlobalPosition = GlobalPosition + new Vector2(HorizontalOffset, VerticalOffset);
	}

    private void ToggleVehicleState()
    {
        if (!Player_In_Boat && _isPlayerNear)
        {
            EnterBoat();
        }

        else if (Player_In_Boat)
        {
            ExitBoat();
        }
        
        StartInputCooldown();
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

        boatLabel.Visible = false;
    }

    private void ExitBoat()
{
    Player_In_Boat = false;

    Vector2 exitPosition = GlobalPosition + new Vector2(-45, 0); 

    _seatMarker.RemoveChild(player);
    GetParent().AddChild(player);

    player.GlobalPosition = exitPosition;

    player.SetPhysicsProcess(true);
    player.SetProcessInput(true);
    player.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false; 
}


    private async void StartInputCooldown()
    {
        _canToggle = false;
        boatLabel.Visible = false;
        await ToSignal(GetTree().CreateTimer(0.2), "timeout");
        _canToggle = true;
        boatLabel.Visible = true;
    }

    private void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			_isPlayerNear = true;
            boatLabel.Visible = true; 
		}
	}

    private void OnBodyExited(Node2D body)
    {
        if (body is Player)
        {
            _isPlayerNear = false;
		}
	}
}