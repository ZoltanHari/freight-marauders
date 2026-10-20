using Godot;

public partial class Boat : CharacterBody2D
{
	[Export] public float Speed { get; set; } = 150f;
    [Export] public float RotationSpeed { get; set; } = 3.0f; 
    [Export] public float VerticalOffset { get; set; } = 35.0f;
    [Export] public float HorizontalOffset { get; set; } = -70.0f;
	private bool _playerInBoat;
	private bool _isPlayerNearBoat;
    private bool _canToggle = true;
	
	Player _player;
    Marker2D _seatMarker; 
    Area2D _area;
	Label _boatLabel;
	Ladder _ladder;
	Node _ogParent;
    

	public override void _Ready()
	{
		_seatMarker = GetNode<Marker2D>("SeatMarker");
		_player = GetNode<Player>("/root/Game/Player");
        _area = GetNode<Area2D>("InteractArea");
        _boatLabel = GetNode<Label>("/root/Game/Boat/BoatText");
        _ladder = GetNode<Ladder>("/root/Game/CargoShip/Ladder");
        _area.BodyEntered += OnBodyEntered;
		_area.BodyExited += OnBodyExited;
		_ogParent = _player.GetParent();
	}
	public override void _PhysicsProcess(double delta)
	{
        if (Input.IsActionJustPressed("interact") && _canToggle)
        {
            ToggleVehicleState();
        }

        if (_playerInBoat)
        {
            float rotationDirection = Input.GetAxis("steer_left", "steer_right");
			Rotation += rotationDirection * RotationSpeed * (float)delta;

			float moveDirection = Input.GetAxis("move_forward", "move_backward");
			Velocity = Transform.Y * moveDirection * Speed;

			MoveAndSlide();
        }

        if (!_playerInBoat && _isPlayerNearBoat)
        {
            StartInputCooldown();
            _boatLabel.Visible = true; 
        }

        else if (!_isPlayerNearBoat)
        {
            _boatLabel.Visible = false;
        }

        _boatLabel.GlobalPosition = GlobalPosition + new Vector2(HorizontalOffset, VerticalOffset);
	}

    private void ToggleVehicleState()
    {
        if (!_playerInBoat && _isPlayerNearBoat && !_ladder.IsPlayerNear)
        {
            EnterBoat();
        }

        else if (_playerInBoat)
        {
            ExitBoat();
        }
        
        StartInputCooldown();
    }

	private void EnterBoat()
    {
        _playerInBoat = true;
        
        _player.SetPhysicsProcess(false); 
        _player.SetProcessInput(false);

        _player.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true; 

        _player.Reparent(this);

        _player.GlobalPosition = _seatMarker.GlobalPosition;

        _boatLabel.Visible = false;
    }

    private void ExitBoat()
{
    _playerInBoat = false;

    Vector2 exitPosition = GlobalPosition + new Vector2(0, 0); 

    _player.Reparent(_ogParent);

    _player.GlobalPosition = exitPosition;

    _player.SetPhysicsProcess(true);
    _player.SetProcessInput(true);
    _player.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false; 
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
			_isPlayerNearBoat = true;
            _boatLabel.Visible = true; 
		}
	}

    private void OnBodyExited(Node2D body)
    {
        if (body is Player)
        {
            _isPlayerNearBoat = false;
		}
	}
}