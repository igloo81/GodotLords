using System;
using System.Collections.Generic;
using Godot;

public static class MapData
{
    public static Lazy<Map> GameMap = new Lazy<Map>(() => Map.FromImage(
        "Resources/map.png", 2, 
        new Dictionary<Color, TerrainType>{
            { Color.FromHtml("#0053c9"), TerrainType.Water},
            { Color.FromHtml("#217725"), TerrainType.Grass},
            { Color.FromHtml("#555555"), TerrainType.Road},
            { Color.FromHtml("#9a5600"), TerrainType.Hill},
            { Color.FromHtml("#693500"), TerrainType.Mountain},
            { Color.FromHtml("#005500"), TerrainType.Forest},
        }
        ));
}