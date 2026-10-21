using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	public const float Speed = 300.0f;
	private var _target;

	public override void _Ready()
	{
		Area2D _area = GetNode<Area2D>("Sight");
		_area.BodyEntered += OnBodyEntered;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_target)
		{
			_attack((float)delta);
		}
	}

	private void _attack(double delta)
	{
		var direction = (_target.GlobalPosition - Position).Normalized;
		Position += direction* Speed * (float)delta;
	}

	private void OnBodyEntered(Node body)
	{
		if (body is Player)
		{
			_target = body;
		}
	}
}