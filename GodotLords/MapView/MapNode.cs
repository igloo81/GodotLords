using Godot;
using System;
using System.Collections.Generic;
using GodotLords.Engine;

namespace GodotLords.MapView;

public partial class MapNode : Node2D, IGameCommandHandler
{
	public GameData GameData { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameData.SomethingHappened += HandleGameCommand;
	}

	public void HandleGameCommand(IGameCommand gameCommand)
	{
		foreach (var child in GetChildren())
		{
			if (child is IGameCommandHandler childAsEventHandler)
				childAsEventHandler.HandleGameCommand(gameCommand);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
