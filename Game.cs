using Godot;
using System;

public static class ObstacleManager
{
    private static PackedScene obstacle = GD.Load<PackedScene>("res://obstacle.tscn");
    private static Random random = new Random();

    public static Node GenerateObstacle(Node2D spawner)
    {
        Vector2 new_position;

        float target_y = spawner.Position.Y + random.Next(-200, 200);
        float target_x = spawner.Position.X;

        new_position.X = target_x;
        new_position.Y = target_y;

        var obstac = obstacle.Instantiate<Obstacle>();

        obstac.Position = new_position;

        return obstac;
    }
}

public static class ScoreManager
{
    private static PackedScene coin_ = GD.Load<PackedScene>("res://coin.tscn");
    private static Random random = new Random();

    public static int current_score = 0;
    public static int high_score = 0;

    public static Label scoreText;

    public static void increase_score(int amount)
    {
        current_score += amount;
        scoreText.Text = current_score.ToString();
    }

    // handles new high scores and returns the high score
    public static int end_score()
    {
        if (current_score > high_score)
            high_score = current_score;
        
        current_score = 0;

        scoreText.Text = "0";

        return high_score;
    }

    public static Node GenerateCoin(Node2D spawner)
    {
        Vector2 new_position;

        float target_y = spawner.Position.Y + random.Next(-200, 200);
        float target_x = spawner.Position.X;

        new_position.X = target_x;
        new_position.Y = target_y;

        var coin = coin_.Instantiate<Coin>();

        coin.Position = new_position;

        return coin;
    }
}

public partial class Game : Node2D
{
    private Node2D spawner;
    private Label scoreText;
    private Control pause_menu;
    private Timer obstacle_timer = new Timer();
    private Random random = new Random();

    private int time_score = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        pause_menu = GetNode<Control>("pausemenu");
        scoreText = GetNode<Label>("score/scoretext");
        ScoreManager.scoreText = scoreText;

        // timer that continously spawns obstacles and coins (name obstaclespawner is a little bit misleading here)
        spawner =
            GetNode<Node2D>("ObstacleSpawner");

        obstacle_timer.WaitTime = 1;
        obstacle_timer.OneShot = false;
        obstacle_timer.Autostart = true;
        obstacle_timer.Timeout += GenerateObstacle;
        AddChild(obstacle_timer);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (Input.IsActionJustPressed("Pause"))
        {
            if (!pause_menu.Visible)
            {
                GetTree().Paused = true;
                pause_menu.Visible = true;
            }
        }
	}

    private void GenerateObstacle()
    {
        Label highscoretext = GetNode<Label>("score/timescore");
        time_score++;
        highscoretext.Text = "time score: " + time_score.ToString();

        int odds = random.Next(0, 31);
        // 33% chance of spawning obstacle, 33% of coin and 33% chance of nothing
        if (odds < 11)
            return;

        if (odds > 10 && odds < 21)
        {
            // spawn an obstacle
            var obstacle = ObstacleManager.GenerateObstacle(spawner);

            AddChild(obstacle);
        } else
        {
            // spawn a coin

            var coin = ScoreManager.GenerateCoin(spawner);

            AddChild(coin);
        }
    }

    private void OnPlayerDied()
    {
        Label highscoretext = GetNode<Label>("deathmenu/highscorelmao");
        highscoretext.Text = ScoreManager.end_score().ToString();
        time_score = 0;
    }

    private void PauseButton()
    {
        GetTree().Paused = false;
        pause_menu.Visible = false;
    }
}
