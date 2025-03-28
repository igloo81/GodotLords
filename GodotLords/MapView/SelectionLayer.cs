using Godot;
using GodotLords.Engine;
using System;
using System.Linq;

namespace GodotLords.MapView;

public partial class SelectionLayer : TileMapLayer
{
    private GameData gameData;
    private Vector2I selectedCell = new Vector2I(-1, -1);
    private ColorRect selectionRectangle;
	public bool UnitSelected { get; set; }
	public bool CitySelected { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        this.gameData = ((MapNode)GetParent()).GameData;
        InitializeSelectionRectangle();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	
    // This function is called when you click on the screen.
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && 
            mouseEvent.Pressed && 
            mouseEvent.ButtonIndex == MouseButton.Left)
        {
            var newSelectedCell = GetTileCoordinates();
			if (newSelectedCell != selectedCell)
			{
				selectedCell = newSelectedCell;
				if (HasUnit())
					UnitSelected = true;
				else if (HasCity())
					CitySelected = true;
			}
			else
			{
				if (HasUnit() && HasCity())
				{
					UnitSelected = !UnitSelected;
					CitySelected = !CitySelected;
				}
				else
				{
					UnitSelected = false;
					CitySelected = false;
				}
				
			}
			UpdateSelectionRect();
        }
    }

	private bool HasCity() =>
		gameData.Cities.Any(c => c.Row == selectedCell.X && c.Column == selectedCell.Y);
	private bool HasUnit() =>
		gameData.UnitsOnMap.Any(u => u.Key == selectedCell);
    private Vector2I GetTileCoordinates()
    {
        Vector2 mousePos = GetGlobalMousePosition();
        Vector2 localPos = ToLocal(mousePos);
        var tileCoords = LocalToMap(localPos);
        return tileCoords;
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

		 if (UnitSelected)
		 {
			selectionRectangle.Color = new Color(1, 1, 0, 0.3f);
		 }
		 else if (CitySelected)
		 {
			selectionRectangle.Color = new Color(0, 1, 1, 0.3f);
		 }
     }   

     private void InitializeSelectionRectangle()
     {        
         selectionRectangle = new ColorRect();
         selectionRectangle.ZIndex = 100;
         AddChild(selectionRectangle);
         selectionRectangle.Visible = false;
     }
}
