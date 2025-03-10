using Godot;
using System;

public partial class Menu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var startButton = GetNode<Button>("Menu/Start");
		startButton.Pressed += OnStartPressed;

		var quitButton = GetNode<Button>("Menu/Quit");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnStartPressed()
	{
		GetTree().ChangeSceneToFile("res://node_2d.tscn");
	}
}
