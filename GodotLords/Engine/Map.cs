using System.Collections.Generic;
using Godot;

namespace GodotLords.Engine;

public partial record Map(TerrainType[][] tiles)
{
    public int Height { get; set; }
    public int Width { get; set; }
    public TerrainType Get(Vector2I position) => tiles[position.Y][position.X];
    public TerrainType Get(int x, int y) => tiles[y][x];

    public static Map FromImage(string path, int pixelsPerTile, Dictionary<Color, TerrainType> colorMapper)
    {
        var image = Image.LoadFromFile(path);
        var width = image.GetSize()[0] / pixelsPerTile;
        var height = image.GetSize()[1] / pixelsPerTile;
        var tiles = new TerrainType[height][];
        for (var y = 0; y < height; y++)
        {
            tiles[y] = new TerrainType[width];
            for (var x = 0; x < width; x++)
            {
                var color = image.GetPixel(x * pixelsPerTile, y * pixelsPerTile);
                tiles[y][x] = colorMapper.ContainsKey(color) ? colorMapper[color] : TerrainType.Water;
            }
        }
        
        return new Map(tiles){ Height = height, Width = width };
    }
}