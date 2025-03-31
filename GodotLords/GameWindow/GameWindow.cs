using System;
using Godot;
using GodotLords.Engine;
using GodotLords.MapView;

namespace GodotLords.GameWindow;

public partial class GameWindow : Control, IGameUpdateHandler
{
	public GameData GameData { get; set; }

	public ArmySelection ArmySelection { get;set; }

	public override void _Ready()
	{
		GameData.SomethingHappened += HandleUpdate;
		ArmySelection = ((PackedScene)ResourceLoader.Load("res://army_selection.tscn")).Instantiate<ArmySelection>();
		GetNode("%BottomPanel").AddChild(ArmySelection);
	}

	public void HandleUpdate(GodotLords.Engine.GameUpdate.IGameUpdate gameUpdate)
	{
		foreach (var child in GetChildren())
		{
			if (child is IGameUpdateHandler childAsEventHandler)
				childAsEventHandler.HandleUpdate(gameUpdate);
		}
	}

	public override void _ExitTree()
	{
		GameData.SomethingHappened -= HandleUpdate;	// todo here or in a constructor?
	}

	public void OnEndTurnPressed()
	{
		GameData.Execute(new Engine.PlayerCommand.EndTurn());
	}
}
