using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] public float Speed { get; set; } = 150f;
    public bool Player_Can_Move = true;

    public override void _Ready()
    {
    }

    public override void _PhysicsProcess(double delta)
    {
        var velocity = Vector2.Zero;
        if (Player_Can_Move == true)
        {
            Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
            if (direction != Vector2.Zero)
            {
                velocity.X = direction.X * Speed * (float)delta;
            }
            else
            {
                velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            }

            if (direction != Vector2.Zero)
            {
                velocity.Y = direction.Y * Speed * (float)delta;
            }
            else
            {
                velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
            }
            var sprite2D = GetNode<Sprite2D>("Sprite2D");

            Position += velocity.Normalized() * Speed * (float)delta;

            if (velocity.X != 0)
            {
                sprite2D.FlipH = false;
                sprite2D.FlipH = velocity.X < 0;
            }
        }

        MoveAndSlide();
    }
}