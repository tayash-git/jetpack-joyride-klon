using Godot;
using System;

public partial class Obstacle : StaticBody2D
{
    static float Speed = 200.0f;

	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
        await ToSignal(GetTree().CreateTimer(13.0), SceneTreeTimer.SignalName.Timeout);
        QueueFree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        Vector2 position = Position;

        position.X -= Speed * (float)delta;

        Position = position;
	}
}
