using Godot;
using System;

public partial class Dingy : CharacterBody2D
{
    [Export] public float Speed = 200.0f;
    [Export] public float Friction = 2.0f;

    public bool IsOccupied { get; private set; } = false;
    private Node2D _currentPassenger;
    private Marker2D _passengerSeat;

    public override void _Ready()
    {
        _passengerSeat = GetNode<Marker2D>("PassengerSeat");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!IsOccupied)
        {
            // Apply friction/deceleration when no one is driving
            Velocity = Velocity.MoveToward(Vector2.Zero, Friction);
            MoveAndSlide();
            return;
        }

        // Handle Dingy Movement Input
        Vector2 inputDir = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (inputDir != Vector2.Zero)
        {
            Velocity = inputDir * Speed;
            // Optional: Rotate boat towards movement direction
            Rotation = inputDir.Angle(); 
        }
        else
        {
            Velocity = Velocity.MoveToward(Vector2.Zero, Friction);
        }

        MoveAndSlide();

        // Keep the hidden player locked to the boat's seat position
        if (_currentPassenger != null)
        {
            _currentPassenger.GlobalPosition = _passengerSeat.GlobalPosition;
        }
    }

    public void BoardDingy(Node2D player)
    {
        IsOccupied = true;
        _currentPassenger = player;
        
        // Disable player physics and visuals
        player.SetPhysicsProcess(false);
        player.Visible = false;
        player.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
    }

    public void ExitDingy()
    {
        if (_currentPassenger == null) return;

        // Re-enable player physics and visuals
        _currentPassenger.SetPhysicsProcess(true);
        _currentPassenger.Visible = true;
        _currentPassenger.GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", false);

        // Place player slightly outside the boat so they don't instantly re-enter
        _currentPassenger.GlobalPosition = GlobalPosition + new Vector2(0, 50); 

        IsOccupied = false;
        _currentPassenger = null;
    }
}