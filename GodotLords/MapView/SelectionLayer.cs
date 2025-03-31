using Godot;
using GodotLords.Engine;
using System;
using System.Linq;

namespace GodotLords.MapView;

public partial class SelectionLayer : TileMapLayer, IGameUpdateHandler
{
    private GameWindow.GameWindow gameWindow;
    private GameData gameData;
    private Vector2I selectedCell = new Vector2I(-1, -1);
    private ColorRect selectionRectangle;
	public bool UnitSelected { get; set; }
	public bool CitySelected { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        this.gameData = GetParent<MapWindow>().GetGameData();
        InitializeSelectionRectangle();
        gameWindow = GetParent<MapWindow>().GetGameWindow();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	
    // This function is called when you click on the screen.
    public override void _Input(InputEvent @event)
    {
        MoveArmy();
        ChangeSelection(@event);
    }

    private void ChangeSelection(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent &&

            mouseEvent.Pressed &&

            mouseEvent.ButtonIndex == MouseButton.Left)
        {
            var newSelectedCell = GetTileCoordinates();
            if (newSelectedCell != selectedCell)
            {
                UnitSelected = false;
                CitySelected = false;
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
            }
            UpdateSelectionRect();
        }
    }


    private void MoveArmy()
    {
        if (UnitSelected)
        {
            if (Input.IsActionPressed("army_move_left"))
                MoveArmy(Vector2I.Left);
            if (Input.IsActionPressed("army_move_right"))
                MoveArmy(Vector2I.Right);
            if (Input.IsActionPressed("army_move_up"))
                MoveArmy(Vector2I.Up);
            if (Input.IsActionPressed("army_move_down"))
                MoveArmy(Vector2I.Down);
        }
    }

    private void MoveArmy(Vector2I direction)
    {
        var destination = selectedCell + direction;
        gameData.Execute(new Engine.PlayerCommand.MoveArmy(selectedCell, destination, gameData.UnitsOnMap[selectedCell]));
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
		 else
		 {
			selectionRectangle.Color = new Color(0, 0, 0, 0.3f);
		 }

        if (UnitSelected)
        {
            gameWindow.ArmySelection.SetArmy(gameData.GetUnits(gameData.UnitsOnMap[selectedCell]).ToArray());
        }
     }   

     private void InitializeSelectionRectangle()
     {        
         selectionRectangle = new ColorRect();
         selectionRectangle.ZIndex = 100;
         AddChild(selectionRectangle);
         selectionRectangle.Visible = false;
     }

    public void HandleUpdate(Engine.GameUpdate.IGameUpdate update)
    {
        switch (update)
        {
            case Engine.GameUpdate.MoveArmy moveArmy:
				if (moveArmy.From == selectedCell)
				{
					selectedCell = moveArmy.To;
                	UpdateSelectionRect();
				}
                break;
        }
    }
}
