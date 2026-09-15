using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] public float Speed = 150.0f;

    private Area2D _interactionArea;
    private Dingy _nearbyDingy;
    private Dingy _currentDingy;

    public override void _Ready()
    {
        // Setup collision detection with the boat's Area2D
        _interactionArea = GetNode<Area2D>("InteractionArea"); // Or handle it from the boat side
    }

    public override void _PhysicsProcess(double delta)
    {
        // 1. Handle Exiting the Dingy
        if (_currentDingy != null && Input.IsActionJustPressed("ui_accept")) // Change "ui_accept" to your Interact key
        {
            _currentDingy.ExitDingy();
            _currentDingy = null;
            return;
        }

        // 2. Handle Entering the Dingy
        if (_nearbyDingy != null && !_nearbyDingy.IsOccupied && Input.IsActionJustPressed("ui_accept"))
        {
            _currentDingy = _nearbyDingy;
            _currentDingy.BoardDingy(this);
            return;
        }

        // 3. Normal Player Movement
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
        if (direction != Vector2.Zero)
        {
            Velocity = direction * Speed;
        }
        else
        {
            Velocity = Velocity.MoveToward(Vector2.Zero, Speed);
        }

        MoveAndSlide();
    }

    // Connect these signals from the Dingy's InteractionArea to the Player, 
    // or use BodyEntered/BodyExited on the Dingy side.
    public void OnDingyAreaEntered(Node2D body)
    {
        if (body is Dingy boat)
        {
            _nearbyDingy = boat;
        }
    }

    public void OnDingyAreaExited(Node2D body)
    {
        if (body is Dingy boat && _nearbyDingy == boat)
        {
            _nearbyDingy = null;
        }
    }
}
