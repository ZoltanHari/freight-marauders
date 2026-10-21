using Godot;

public partial class Enemy : CharacterBody2D
{
	[Export] public float Speed { get; set; } = 60f;
	private bool _playerInZone;
	Player _target;
	Player _player;
	RayCast2D _raycast;
	
	
	public override void _Ready()
	{
		Area2D area = GetNode<Area2D>("Sight");
		_raycast = GetNode<RayCast2D>("Sight/RayCast2D");
		_player = GetNode<Player>("/root/Game/Player");
		area.BodyEntered += OnBodyEntered;
		area.BodyExited += OnBodyExited;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_playerInZone)
		{
			_raycast.TargetPosition = _raycast.ToLocal(_player.GlobalPosition);
			_raycast.ForceRaycastUpdate();
		}

		if (_raycast.IsColliding())
		{
			var collider = _raycast.GetCollider();
			if (collider == _player)
			{
				Vector2 direction = (_player.GlobalPosition - GlobalPosition).Normalized();
				Velocity = direction * Speed;
				MoveAndSlide();
				
			}
		}
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			_target = _player;
			_playerInZone = true;
		}
	}
	
	private void OnBodyExited(Node body)
	{
		if (body is Player)
		{
			_target = null;
			_playerInZone = false;
		}
	} 
}