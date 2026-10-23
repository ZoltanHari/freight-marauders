using Godot;

public partial class Enemy : CharacterBody2D
{
	[Export] public float Speed { get; set; } = 60f;
	Player _target;
	Player _player;
	NavigationAgent2D _pathfinding;
	Timer _timer;
	
	public override void _Ready()
	{
		Area2D area = GetNode<Area2D>("Sight");
		_player = GetNode<Player>("/root/Game/Player");
		_pathfinding = GetNode<NavigationAgent2D>("PathFinding");
		_timer = GetNode<Timer>("Timer");
		_timer.Timeout += OnTimerTimeout;
		area.BodyEntered += OnBodyEntered;
		area.BodyExited += OnBodyExited;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = (_pathfinding.GetNextPathPosition() - GlobalPosition).Normalized();
		Velocity = Speed * direction;
		MoveAndSlide();
	}

	private void MakePath()
	{
		_pathfinding.TargetPosition = _player.GlobalPosition;
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			_target = _player;
		}
	}
	
	private void OnBodyExited(Node body)
	{
		if (body is Player)
		{
			_target = null;
		}
	}

	private void OnTimerTimeout()
	{
		MakePath();
	}
}