using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using GodotLords.Engine;

namespace GodotLords.MapView;

public partial class UnitsMapLayer : TileMapLayer, IGameCommandHandler    // todo this should be a set of node2ds
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
            ShowArmyOnMap(unitOnMap.Key, unitOnMap.Value);

        InitializeSelectionRectangle();
    }

    private void InitializeSelectionRectangle()
    {        
        selectionRectangle = new ColorRect();
        selectionRectangle.Color = new Color(1, 1, 0, 0.3f); // todo move the selection rectangle outside of this class
        AddChild(selectionRectangle);
        selectionRectangle.Visible = false;
    }
    
	public void HandleGameCommand(IGameCommand gameCommand)
	{
        switch (gameCommand)
        {
            case MoveArmy move:     // should be enriched in gameData with units etc? Yeah, I do need them here :-)
                RemoveArmyOnMap(move.From);
                ShowArmyOnMap(move.To);
                break;

            case MovePartOfArmy movePart: 
                break;
        }
	}

    private void RemoveArmyOnMap(Vector2I position)
    {
        if (unitsOnScreen.TryGetValue(position, out var army))
            RemoveChild(army);
    }

    private void ShowArmyOnMap(Vector2I position, string[] unitIds)
    {
        var tileSize = 32; // todo share!
        var units = gameData.GetUnits(unitIds).ToArray();

        var newArmyScene = armyScene.Instantiate<ArmyScene>();
        newArmyScene.Units = units;
        newArmyScene.Position = position * tileSize + new Vector2I(tileSize / 2, tileSize / 2);

        unitsOnScreen[position] = newArmyScene;
        AddChild(newArmyScene);
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

        RemoveArmyOnMap(selectedCell);
    }
} 
