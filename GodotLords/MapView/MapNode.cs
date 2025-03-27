using Godot;
using System;
using System.Collections.Generic;
using GodotLords.Engine;

namespace GodotLords.MapView;

public partial class MapNode : Node2D, IGameUpdateHandler
{
	public GameData GameData { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameData.SomethingHappened += HandleUpdate;
	}

	public void HandleUpdate(GodotLords.Engine.GameUpdate.IGameUpdate gameUpdate)
	{
		foreach (var child in GetChildren())
		{
			if (child is IGameUpdateHandler childAsEventHandler)
				childAsEventHandler.HandleUpdate(gameUpdate);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _ExitTree()
	{
		GameData.SomethingHappened -= HandleUpdate;	// todo here or in a constructor?
	}
}
