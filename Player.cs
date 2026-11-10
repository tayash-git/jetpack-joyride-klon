using Godot;
using System;

class PlayerData
{
    public float jetpack_power = 3000.0f;
    public int coins = 0;
    
    private float gravity_multiplier = 2.0f;
    private float target_x = 0;

    public void init(float target)
    {
        target_x = target;
    }

    // Movement functions
    private void ProcessGravity(ref Vector2 velocity, double delta_time)
    {
        
        float gravity = (float)ProjectSettings.GetSetting("physics/2d/default_gravity");

        velocity.Y += gravity * gravity_multiplier * (float)delta_time;
    }

    public void ProcessMovement(
        ref Vector2 position,
        ref Vector2 velocity,
        double delta_time
    )
    {
        // prevent player from moving sideways
        position.X = target_x;

        // Processing player jump and clamping the value cause we don't want
        // the y velocity to be able to be super high (ignore gravity too)
        if (Input.IsActionPressed("JetpackJump"))
        {
            velocity.Y += -1 * jetpack_power * (float)delta_time;
        }
        else
        {
            ProcessGravity(ref velocity, delta_time); // Process gravity if not jumping
        }
    }
}

public partial class Player : CharacterBody2D
{
    [Signal]
    public delegate void CoinTouchEventHandler();

    [Signal]
    public delegate void PlayerDiedEventHandler();

    private PlayerData player_data;

    public override void _Ready()
    {
        player_data = new PlayerData();
        player_data.init(Position.X);
    }

    public override void _Process(double delta)
    {
        MoveAndSlide();
        
        Vector2 velocity = Velocity;
        Vector2 position = Position;

        player_data.ProcessMovement(ref position, ref velocity, delta);

        Velocity = velocity;
        Position = position;
    }

    public void OnBodyEntered(Node2D body)
    {
        switch (body)
        {
            case Obstacle:
                EmitSignal(SignalName.PlayerDied);
                Node2D main = (Node2D)GetParent();
                Control gui = main.GetNode<Control>("deathmenu");

                gui.Visible = true;
                GetTree().Paused = true;
                
                break;

            case Coin:
                ScoreManager.increase_score(1);
                body.QueueFree();
                break;
        }
    }
}
