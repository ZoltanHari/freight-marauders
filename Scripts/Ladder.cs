using Godot;

public partial class Ladder : Area2D
{ 
	public bool IsPlayerNear; 
	private bool _inShip;
	
	Player _player;
	Marker2D _enterShip; 
	Marker2D _exitShip;
	Node _ogParent;
	StaticBody2D _cargoShip;
	
	public override void _Ready()
	{
		_player = GetNode<Player>("/root/Game/Player");
		_enterShip = GetNode<Marker2D>("../EnterShip");
		_exitShip = GetNode<Marker2D>("../ExitShip");
		_cargoShip = GetNode<StaticBody2D>("/root/Game/CargoShip");
		_ogParent = _player.GetParent();
		BodyEntered += OnBodyEntered;
	}

	public override void _PhysicsProcess(double delta)
{
    if (Input.IsActionJustPressed("interact") && IsPlayerNear && !_inShip)
    {
        _player.Reparent(_cargoShip);
        _player.GlobalPosition = _enterShip.GlobalPosition;

        IsPlayerNear = false;
        _inShip = true;
    }
    
    else if (Input.IsActionJustPressed("interact") && IsPlayerNear && _inShip)
    {
        _player.Reparent(_ogParent);
        _player.GlobalPosition = _exitShip.GlobalPosition;

        IsPlayerNear = false;
        _inShip = false;
    }
}

	private void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			IsPlayerNear = true;
		}
	}
}
