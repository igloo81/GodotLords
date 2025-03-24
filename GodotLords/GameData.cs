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

public enum UnitTypeEnum    // should be in a data file? Well, some are special and need to be known, so nope
{
    LightInfantry,
    HeavyInfantry,
    Giant,
    Archer,
    WolfRider,
    Cavalry,
    Pegasus,
    Griffin,
    Dwarf,
    Navy,
    Ghost,
    Demon,
    Devil,
    Wizard,
    Dragon,
    Knight
}

public class Unit
{
    public int Upkeep { get; set; }
    int Strength { get; set;}
    int Moves { get; set; }

    // city / player

    public static TerrainType[] TerrainTypes = new TerrainType[] { 
        TerrainType.Road, TerrainType.Bridge, TerrainType.Water, TerrainType. Shore, TerrainType.Forest, 
        TerrainType.Hill, TerrainType.Mountain, TerrainType.Grass, TerrainType.Swamp, TerrainType.City};
    public static Dictionary<UnitTypeEnum, int[]> movementSpeeds = new Dictionary<UnitTypeEnum, int[]>
    {
        { UnitTypeEnum.Giant, new int[] { 1, 1, -1, -1, 5, 4, -1, 2, 5, 1 } },
        { UnitTypeEnum.Dwarf, new int[] { 1, 1, -1, -1, 6, 3, -1, 2, 6, 1 } },
        { UnitTypeEnum.HeavyInfantry, new int[] { 1, 1, -1, -1, 4, 6, -1, 2, 5, 1 } },
        { UnitTypeEnum.LightInfantry, new int[] { 1, 1, -1, -1, 4, 6, -1, 2, 5, 1 } },
        { UnitTypeEnum.Archer, new int[] { 1, 1, -1, -1, 2, 5, -1, 2, 6, 1 } },
        { UnitTypeEnum.Cavalry, new int[] { 1, 1, -1, -1, 5, 6, -1, 2, 5, 1 } },
        { UnitTypeEnum.WolfRider, new int[] { 1, 1, -1, -1, 4, 6, -1, 2, 4, 1 } },
        { UnitTypeEnum.Pegasus, new int[] { 2, 2, 2, 2, 2, 2, 3, 2, 2, 2 } },
        { UnitTypeEnum.Griffin, new int[] { 2, 2, 2, 2, 2, 2, 3, 2, 2, 2 } },
        { UnitTypeEnum.Ghost, new int[] { 1, 1, -1, -1, 4, 5, -1, 2, 4, 1 } },
        { UnitTypeEnum.Demon, new int[] { 1, 1, -1, -1, 4, 5, -1, 2, 5, 1 } },
        { UnitTypeEnum.Devil, new int[] { 1, 1, -1, -1, 4, 5, -1, 2, 5, 1 } },
        { UnitTypeEnum.Dragon, new int[] { 2, 2, 2, 2, 2, 2, 3, 2, 2, 2 } },
        { UnitTypeEnum.Wizard, new int[] { 1, 1, -1, -1, 4, 6, -1, 2, 5, 1 } },
        { UnitTypeEnum.Knight, new int[] { 1, 1, -1, -1, 4, 6, -1, 2, 5, 1 } },
        { UnitTypeEnum.Navy, new int[] { -1, 2, 1, 2, -1, -1, -1, -1, -1, -1 } }
    };
}
