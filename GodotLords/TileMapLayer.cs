using Godot;
using System;
using System.Linq;

public partial class TileMapLayer : Godot.TileMapLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        var map = Map.FromImage("resources/map.png", water:Color.FromHtml("#0053c9"), road: Color.FromHtml("#555555"));
        var map2 = Map.FromImage("resources/mapFromForum.png", water:Color.FromHtml("#ff009eba"), road: Color.FromHtml("#555555"));

		for (var x = 1; x < map.Width; x++)
			for (var y = 1; y < map.Height; y++)
			{
                var tileIndex = GetTileIndex(new int[] { map.Get(x-1,y-1), map.Get(x,y-1), map.Get(x-1,y), map.Get(x,y)});
                Console.WriteLine(tileIndex);
				this.SetCell(new Vector2I(x, y), 2, tileIndex);
			}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private Vector2I GetTileIndex(int[] neighbours)
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

        var index = (neighbours[0] != 0 ? 1 : 0) + (neighbours[1] != 0 ? 1 : 0)*2 + (neighbours[2] != 0 ? 1 : 0)*4 + (neighbours[3] != 0 ? 1 : 0)*8;
        var result = offsets[index];          
        return new Vector2I(result.Item1, result.Item2);


        // if (neighbours.All(_ => _ == 0 || _ == 1))
        // {
        //     var index = neighbours[0] + neighbours[1]*2 + neighbours[2]*4 + neighbours[3]*8;
	    // 	var result = offsets[index];          
        //     return new Vector2I(result.Item1, result.Item2);
        // } else if (neighbours.All(_ => _ == 1 || _ == 2))
        // {
        //     var index = neighbours[0]-1 + (neighbours[1]-1)*2 + (neighbours[2]-1)*4 + (neighbours[3]-1) *8;
        //     var x = index % 4;
        //     var y = index / 4;
	    // 	var result = offsets[index];          
        //     return new Vector2I(x+5, y);
        // } else
        //     return new Vector2I(4, 1);
	}
}

public record Map(int[][] tiles)
{
    public int Height { get; set; }
    public int Width { get; set; }
    public int Get(int x, int y) => tiles[y][x];

    public static Map FromImage(string path, Color water, Color road)
    {
        var image = Image.LoadFromFile(path);
        var width = image.GetSize()[0];
        var height = image.GetSize()[1];
        var tiles = new int[height][];
        for (var y = 0; y < height; y++)
        {
            tiles[y] = new int[width];
            for (var x = 0; x < width; x++)
            {
                var color = image.GetPixel(x, y);
                var isWater = color == water; 
                var isRoad = color == road;
                tiles[y][x] = isWater ? 0 : (isRoad ? 2 : 1);
            }
        }
        
        return new Map(tiles){ Height = height, Width = width };
    }
}