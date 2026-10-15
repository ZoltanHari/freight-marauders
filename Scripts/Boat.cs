using Godot;
using System;

public partial class Boat : CharacterBody2D
{
	[Export] public float Speed { get; set; } = 150f;
    [Export] public float RotationSpeed { get; set; } = 3.0f; 
    [Export] public float VerticalOffset { get; set; } = 35.0f;
    [Export] public float HorizontalOffset { get; set; } = -70.0f;
	public bool PlayerInBoat = false;
	private bool _isPlayerNear = false;
    private bool _canToggle = true;

	public Player player;
    private Marker2D _seatMarker; 
    private Area2D _area;
    private Label boatLabel;

	public override void _Ready()
	{
		_seatMarker = GetNode<Marker2D>("SeatMarker");
		player = GetNode<Player>("/root/Game/Player");
        _area = GetNode<Area2D>("InteractArea");
        boatLabel = GetNode<Label>("/root/Game/Boat/BoatText");
        _area.BodyEntered += OnBodyEntered;
		_area.BodyExited += OnBodyExited;
	}
	public override void _PhysicsProcess(double delta)
	{
        if (Input.IsActionJustPressed("interact") && _canToggle)
        {
            ToggleVehicleState();
        }

        if (PlayerInBoat)
        {
            float rotationDirection = Input.GetAxis("steer_left", "steer_right");
			Rotation += rotationDirection * RotationSpeed * (float)delta;

			float moveDirection = Input.GetAxis("move_forward", "move_backward");
			Velocity = Transform.Y * moveDirection * Speed;

			MoveAndSlide();
        }

        if (!PlayerInBoat && _isPlayerNear)
        {
            StartInputCooldown();
            boatLabel.Visible = true; 
        }

        else if (!_isPlayerNear)
        {
            boatLabel.Visible = false;
        }

        boatLabel.GlobalPosition = GlobalPosition + new Vector2(HorizontalOffset, VerticalOffset);
	}

    private void ToggleVehicleState()
    {
        if (!PlayerInBoat && _isPlayerNear)
        {
            EnterBoat();
        }

        else if (PlayerInBoat)
        {
            ExitBoat();
        }
        
        StartInputCooldown();
    }

	private void EnterBoat()
    {
        PlayerInBoat = true;
        
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
    PlayerInBoat = false;

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
        await ToSignal(GetTree().CreateTimer(0.2), "timeout");
        _canToggle = true;
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