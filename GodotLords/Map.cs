using Godot;

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