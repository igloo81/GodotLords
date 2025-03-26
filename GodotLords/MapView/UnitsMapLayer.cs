using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class UnitsMapLayer : TileMapLayer   // todo this should be a set of node2ds
{
    private Vector2I selectedCell = new Vector2I(-1, -1);
    private ColorRect selectionRectangle;
    private GameData gameData;
    private PackedScene armyScene;

    private Dictionary<Vector2I, Node2D> unitsOnScreen;

    public override void _Ready()
    {
        this.unitsOnScreen = new Dictionary<Vector2I, Node2D>();
        this.gameData = ((MapNode)GetParent()).GameData;
        this.armyScene = (PackedScene)ResourceLoader.Load("res://ArmyScene.tscn");

        foreach (var unitOnMap in gameData.UnitsOnMap)  // todo accessing these objects async...
            ShowUnitOnMap(unitOnMap.Key, unitOnMap.Value);

        InitializeSelectionRectangle();
    }

    private void InitializeSelectionRectangle()
    {        
        selectionRectangle = new ColorRect();
        selectionRectangle.Color = new Color(1, 1, 0, 0.3f); // todo move the selection rectangle outside of this class
        AddChild(selectionRectangle);
        selectionRectangle.Visible = false;
    }

    private void ShowUnitOnMap(Vector2I position, string[] unitIds)
    {
        var tileSize = 32; // todo share!
        var units = gameData.GetUnits(unitIds).ToArray();

        var newArmyScene = armyScene.Instantiate<ArmyScene>();
        newArmyScene.Units = units;
        newArmyScene.Position = position * tileSize + new Vector2I(tileSize / 2, tileSize / 2);
        AddChild(newArmyScene);

        var unitTypeToShow = GetUnitTypeToShow(units.Select(_ => _.unitTypeEnum).ToArray());
        this.SetCell(position, 0, GetOffsetInTileSheet(unitTypeToShow));    // todo show flag
    }

    private Vector2I GetOffsetInTileSheet(UnitTypeEnum unitTypeEnum) => unitTypeEnum switch
    {
        UnitTypeEnum.LightInfantry => new Vector2I(0, 0),
        UnitTypeEnum.HeavyInfantry => new Vector2I(1, 0),
        UnitTypeEnum.Giant => new Vector2I(2, 0),
        UnitTypeEnum.Archer => new Vector2I(3, 0),

        UnitTypeEnum.WolfRider => new Vector2I(0, 1),
        UnitTypeEnum.Cavalry => new Vector2I(1, 1),
        UnitTypeEnum.Pegasus => new Vector2I(2, 1),
        UnitTypeEnum.Griffin => new Vector2I(3, 1),

        UnitTypeEnum.Dwarf => new Vector2I(0, 2),
        UnitTypeEnum.Navy => new Vector2I(1, 2),
        UnitTypeEnum.Ghost => new Vector2I(2, 2),
        UnitTypeEnum.Demon => new Vector2I(3, 2),

        UnitTypeEnum.Devil => new Vector2I(0, 3),
        UnitTypeEnum.Wizard => new Vector2I(1, 3),
        UnitTypeEnum.Dragon => new Vector2I(2, 3),
        UnitTypeEnum.Knight => new Vector2I(3, 3),

        _ => throw new NotImplementedException()
    };

    private UnitTypeEnum GetUnitTypeToShow(UnitTypeEnum[] unitTypes)
    {

        if (unitTypes.Length == 0)
            throw new Exception($"Can't determine which unit to show if not unitTypes are passed");

         return unitTypes.OrderByDescending(_ => GetOffsetInTileSheet(_).X + GetOffsetInTileSheet(_).Y * 3).First(); // todo compute twice :-)
    }

    // This function is called when you click on the screen.
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && 
            mouseEvent.Pressed && 
            mouseEvent.ButtonIndex == MouseButton.Left)
        {
            // Get the mouse position in the global space
            Vector2 mousePos = GetGlobalMousePosition();

            // Convert the mouse position to the local space of the TileMap
            Vector2 localPos = ToLocal(mousePos);

            // Convert the local position to tile coordinates
            var tileCoords = LocalToMap(localPos);

            // Print the tile coordinates to the console
            GD.Print("Tile selected at: " + tileCoords);

            // Update selection
            selectedCell = tileCoords;
            UpdateSelectionRect();

            // Optionally, get the tile ID at that position
            //int tileId = GetCellSourceId(0, tileCoords);
            //GD.Print("Tile ID: " + tileId);

            // You can also replace the tile with another ID, for example:
            // SetCell(0, tileCoords, 0, new Vector2I(0, 0));
        }
    }    

    private void UpdateSelectionRect()
    {
        if (selectedCell.X < 0 || selectedCell.Y < 0)
        {
            selectionRectangle.Visible = false;
            return;
        }

        // Get the tile size
        var tileSize = TileSet.TileSize;
        
        // Position the selection rectangle
        selectionRectangle.Position = MapToLocal(selectedCell) - tileSize / 2;
        selectionRectangle.Size = tileSize;
        selectionRectangle.Visible = true;
    }
} 
