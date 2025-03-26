using Godot;
using System.Collections.Generic;

public partial class UnitsMapLayer : TileMapLayer
{
    private Vector2I _selectedCell = new Vector2I(-1, -1);
    private ColorRect _selectionRect;

    public override void _Ready()
    {
        var gameData = ((MapNode)GetParent()).GameData;

        foreach (var unitOnMap in gameData.UnitsOnMap)
        {
            var position = unitOnMap.Key;
            var unitIds = unitOnMap.Value;
            this.SetCell(position, 0, new Vector2I(0, 0));
        }


        // Create a selection rectangle
        _selectionRect = new ColorRect();
        _selectionRect.Color = new Color(1, 1, 0, 0.3f); // Semi-transparent yellow
        AddChild(_selectionRect);
        _selectionRect.Visible = false;
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
            _selectedCell = tileCoords;
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
        if (_selectedCell.X < 0 || _selectedCell.Y < 0)
        {
            _selectionRect.Visible = false;
            return;
        }

        // Get the tile size
        var tileSize = TileSet.TileSize;
        
        // Position the selection rectangle
        _selectionRect.Position = MapToLocal(_selectedCell) - tileSize / 2;
        _selectionRect.Size = tileSize;
        _selectionRect.Visible = true;
    }
} 
