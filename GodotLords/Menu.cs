using Godot;
using System;

public partial class Menu : Control
{
	[Export] public Color HoverColor = new Color(0.2f, 0.2f, 0.2f); // Lighter color on hover

	public override void _Ready()
	{
		var startButton = GetNode<Button>("Menu/Start");
		startButton.Pressed += OnStartPressed;

		var quitButton = GetNode<Button>("Menu/Quit");
		quitButton.Pressed += OnQuitPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnStartPressed()
	{
		GetTree().ChangeSceneToFile("res://node_2d.tscn");
	}

	public void OnQuitPressed()
	{
		GetTree().Quit();
	}	
}
