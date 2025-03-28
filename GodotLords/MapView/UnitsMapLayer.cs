using Godot;
using GodotLords.Engine;
using System.Collections.Generic;
using System.Linq;

namespace GodotLords.MapView;

public partial class UnitsMapLayer : TileMapLayer
{
    private GameData gameData;
    private PackedScene armyScene;
    private Dictionary<Vector2I, Node2D> unitsOnScreen;

    public override void _Ready()
    {
        this.unitsOnScreen = new Dictionary<Vector2I, Node2D>();
        this.gameData = ((MapNode)GetParent()).GameData;
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
} 
