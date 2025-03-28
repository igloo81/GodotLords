using Godot;
using System;
using System.Linq;
using GodotLords.Engine;

namespace GodotLords.MapView;

public partial class TerrainMapLayer : Godot.TileMapLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        var map = GetParent<MapWindow>().GetGameData().Map;
        //var map = Map.FromImage("Resources/map.png", 2, water:Color.FromHtml("#0053c9"), road: Color.FromHtml("#555555"));
        //var map = Map.FromImage("Resources/mapFromForum.png", 2, water:Color.FromHtml("#ff009eba"), road: Color.FromHtml("#555555"));

        
		for (var x = 1; x < map.Width; x++)
			for (var y = 1; y < map.Height; y++)
			{
                var tileIndex = GetTileIndex(new TerrainType[] { map.Get(x-1,y-1), map.Get(x,y-1), map.Get(x-1,y), map.Get(x,y)});
				this.SetCell(new Vector2I(x, y), 2, tileIndex);
			}
            
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private Vector2I GetTileIndex(TerrainType[] neighbours)
	{
        var offsets = new (int, int)[] {
            (2, 0),
            (1, 1),
            (0, 1),
            (2, 3),
            (1, 0),
            (3, 3),
            (2, 1),
            (0, 2),
            (0, 0),
            (3, 1),
            (3, 2),
            (1, 2),
            (2, 2),
            (0, 3),
            (1, 3),
            (3, 0),
        };

        var index = (neighbours[0] != TerrainType.Water ? 1 : 0) + (neighbours[1] != TerrainType.Water ? 1 : 0)*2 + (neighbours[2] != TerrainType.Water ? 1 : 0)*4 + (neighbours[3] != TerrainType.Water ? 1 : 0)*8;
        var result = offsets[index];          
        return new Vector2I(result.Item1, result.Item2);


        // if (neighbours.All(_ => _ == 0 || _ == 1))
        // {
        //     var index = neighbours[0] + neighbours[1]*2 + neighbours[2]*4 + neighbours[3]*8;
	    // 	var result = offsets[index];          
        //     return new Vector2I(result.Item1, result.Item2);
        // } else if (neighbours.All(_ => _ == 1 || _ == 2))
        // {
        //     return new Vector2I(x+5, y);
        // } else
        //     return new Vector2I(4, 1);
	}
}
