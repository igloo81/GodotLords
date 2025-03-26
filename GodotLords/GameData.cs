using System;
using System.Collections.Generic;
using Godot;

public class GameData
{
    public Map Map { get; set;}
    public Dictionary<Vector2I, string[]> UnitsOnMap { get; set; }
    public List<Unit> Units { get; set; }
    public List<City> Cities { get; set; }
}

public record City(string Name);    // units to produce, row, column, defense etc.

public record UnitOnMap(int Row, int Column, string Id);        // todo reference to unit, not unit itself

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
    public string Id { get; set; }
    public int Upkeep { get; set; }
    public int Strength { get; set;}
    public int MovesMaximum { get; set; }
    public int MovesLeft { get; set; }
    public string PlayerId { get; set;}
    public UnitTypeEnum unitTypeEnum { get; set; }

    public static int GetMovementCosts(UnitTypeEnum unitType, TerrainType terrainType)
    {
        var terrainIndex = Array.IndexOf(TerrainTypes, terrainType);
        return movementSpeeds[unitType][terrainIndex];
    }

    // city / player

    private static TerrainType[] TerrainTypes = new TerrainType[] { 
        TerrainType.Road, TerrainType.Bridge, TerrainType.Water, TerrainType. Shore, TerrainType.Forest, 
        TerrainType.Hill, TerrainType.Mountain, TerrainType.Grass, TerrainType.Swamp, TerrainType.City};
    private static Dictionary<UnitTypeEnum, int[]> movementSpeeds = new Dictionary<UnitTypeEnum, int[]>
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
