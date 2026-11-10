using Godot;
using System;

public partial class Main : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

    public void OnButtonDown()
    {
		GetTree().Paused = false;
        GetTree().ReloadCurrentScene();

        GetNode<Node2D>("Game").GetNode<Control>("deathmenu").Visible = false;
    }
}
