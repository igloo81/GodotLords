using Godot;
using GodotLords.Engine;

namespace GodotLords.MapView;

public partial class MapWindow : Control, IGameUpdateHandler
{
    public GameData GetGameData()
	{
		var parent = GetParent();
		while (parent != null && !(parent is GameWindow.GameWindow))
			parent = parent.GetParent();
		return ((GameWindow.GameWindow)parent).GameData;
	}

	public override void _Ready()
	{
		GetGameData().SomethingHappened += HandleUpdate;
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
		GetGameData().SomethingHappened -= HandleUpdate;	// todo here or in a constructor?
	}
}
