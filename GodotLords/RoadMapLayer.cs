using Godot;
using System;

public partial class RoadMapLayer : TileMapLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        var map = Map.FromImage("Resources/map.png", water:Color.FromHtml("#0053c9"), road: Color.FromHtml("#555555"));
		for (var x = 1; x < map.Width; x++)
			for (var y = 1; y < map.Height; y++)
			{
                var tileIndex = new Vector2I(4, 0);
				this.SetCell(new Vector2I(x, y), 0, tileIndex);
			}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
