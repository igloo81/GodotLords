using Godot;
using System.Collections.Generic;

public partial class UnitsMapLayer : TileMapLayer
{
    public override void _Ready()
    {
        this.SetCell(new Vector2I(10, 10), 0, new Vector2I(0, 0));
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

            // Optionally, get the tile ID at that position
            //int tileId = GetCellSourceId(0, tileCoords);
            //GD.Print("Tile ID: " + tileId);

            // You can also replace the tile with another ID, for example:
            // SetCell(0, tileCoords, 0, new Vector2I(0, 0));
        }
    }    
} 
