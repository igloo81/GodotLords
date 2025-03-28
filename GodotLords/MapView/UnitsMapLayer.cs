using Godot;
using GodotLords.Engine;
using GodotLords.Engine.GameUpdate;
using System.Collections.Generic;
using System.Linq;

namespace GodotLords.MapView;

public partial class UnitsMapLayer : TileMapLayer, IGameUpdateHandler
{
    private GameData gameData;
    private PackedScene armyScene;
    private Dictionary<Vector2I, Node2D> unitsOnScreen;

    public override void _Ready()
    {
        this.unitsOnScreen = new Dictionary<Vector2I, Node2D>();
        this.gameData = GetParent<MapWindow>().GetGameData();
        this.armyScene = (PackedScene)ResourceLoader.Load("res://ArmyScene.tscn");

        foreach (var unitOnMap in gameData.UnitsOnMap)  // todo accessing these objects async...
             ShowArmyOnMap(unitOnMap.Key, gameData.GetUnits(unitOnMap.Value).ToArray());
    }

    private void ShowArmyOnMap(Vector2I position, Unit[] units)
    {
         var tileSize = 32; // todo share!
 
         var newArmyScene = armyScene.Instantiate<ArmyScene>();
         newArmyScene.Units = units;
         newArmyScene.Position = position * tileSize + new Vector2I(tileSize / 2, tileSize / 2);
 
         unitsOnScreen[position] = newArmyScene;
         AddChild(newArmyScene);
     }

    private void RemoveArmyFromMap(Vector2I position)
    {
        if (unitsOnScreen.TryGetValue(position, out var armyScene))
        {
            RemoveChild(armyScene);
            armyScene.QueueFree();
            unitsOnScreen.Remove(position);
        }
    }

    public void HandleUpdate(IGameUpdate update)
    {
        switch (update)
        {
            case Engine.GameUpdate.MoveArmy moveArmy:
                RemoveArmyFromMap(moveArmy.From);
                if (moveArmy.UnitsLeft.Length > 0)
                    ShowArmyOnMap(moveArmy.From, moveArmy.UnitsLeft);   // todo handle armies walking over each other
                ShowArmyOnMap(moveArmy.To, moveArmy.UnitsMoved);
                break;
        }
    }
} 
