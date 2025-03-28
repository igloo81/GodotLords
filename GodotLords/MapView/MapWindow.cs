using Godot;
using GodotLords.Engine;

namespace GodotLords.MapView;

public partial class MapWindow : Control, IGameUpdateHandler
{
    public GameData GetGameData() =>  
        ((GameWindow.GameWindow)GetParent()).GameData;

	public void HandleUpdate(GodotLords.Engine.GameUpdate.IGameUpdate gameUpdate)
	{
		foreach (var child in GetChildren())
		{
			if (child is IGameUpdateHandler childAsEventHandler)
				childAsEventHandler.HandleUpdate(gameUpdate);
		}
	}
}
