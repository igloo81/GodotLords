using Godot;
using GodotLords.Engine;

namespace GodotLords.GameWindow;

public partial class GameWindow : Control, IGameUpdateHandler
{
	public GameData GameData { get; set; }

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

	public override void _ExitTree()
	{
		GameData.SomethingHappened -= HandleUpdate;	// todo here or in a constructor?
	}
}
