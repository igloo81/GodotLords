using Godot;
using System;

public partial class RoadMapLayer : TileMapLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        var map = MapData.GameMap.Value;
        //var map = Map.FromImage("Resources/map.png", 2, water:Color.FromHtml("#0053c9"), road: Color.FromHtml("#555555"));
        //var map = Map.FromImage("Resources/mapFromForum.png", 2, water:Color.FromHtml("#ff009eba"), road: Color.FromHtml("#555555"));

		for (var x = 1; x < map.Width; x++)
			for (var y = 1; y < map.Height; y++)
			{
				if (map.Get(x, y) == 2)	// todo enums for types
				{					
		            //var tileIndex = GetTileIndex(map, x, y);
					var tileIndex = new Vector2I(4, 1);	// We only use the tile that covers the full 32x32 sprite
					this.SetCell(new Vector2I(x, y), 0, tileIndex);
				}
			}
	}

	// Returns the roads that are centered in the sprite instead of a full 32x32 sprite
	private Vector2I GetTileIndex(Map map, int x, int y)
	{
		var neighbours = new[] { map.Get(x,y-1), map.Get(x+1,y), map.Get(x,y+1), map.Get(x-1,y)};
        var index = (neighbours[0] == 2 ? 1 : 0) + (neighbours[1] == 2 ? 1 : 0)*2 + (neighbours[2] == 2 ? 1 : 0)*4 + (neighbours[3] == 2 ? 1 : 0)*8;
		var tileX = index % 4;
		var tileY = index / 4;
        return new Vector2I(tileX+5, tileY);
	}
}
