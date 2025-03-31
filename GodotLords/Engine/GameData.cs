using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
//using GodotLords.Engine.GameUpdate;
//using GodotLords.Engine.PlayerCommand;

namespace GodotLords.Engine;

public class GameData
{
    public Map Map { get; set;}
    public Dictionary<Vector2I, string[]> UnitsOnMap { get; set; }
    public List<Unit> Units { get; set; }
    
    public List<City> Cities { get; set; }

    public string[] Players { get; set; }

    private int currentPlayerIndex = 0;
    public string CurrentPlayer => Players[currentPlayerIndex];

    public IEnumerable<Unit> GetUnits(string[] ids)
    {
        foreach (var id in ids)
        {
            var unit = Units.FirstOrDefault(_ => _.Id == id);
            if (unit == null)
                throw new NotImplementedException($"can't find unit with id {id}");
            yield return unit;
        }
    }

    public delegate void GameEventHandler(GodotLords.Engine.GameUpdate.IGameUpdate gameEventArgs);
    public event GameEventHandler SomethingHappened;

    public void Execute(GodotLords.Engine.PlayerCommand.IPlayerCommand command)
    {
        // todo chain commands!
        switch (command)
        {
            case PlayerCommand.MoveArmy move:
                Move(move);
                break;
            case PlayerCommand.EndTurn endTurn:
                EndTurn(endTurn);
                break;

            default:
                throw new NotImplementedException($"{command.GetType()}");
        }
    }

    private void Move(PlayerCommand.MoveArmy move)
    {
        var unitsOnTile = UnitsOnMap[move.From];
        var unitsLeft = unitsOnTile.Except(move.UnitIds).ToArray();
        var unitsMoved = GetUnits(move.UnitIds).Select(_ => _.MoveTo(Map.Get(move.To))).ToArray();
        if (unitsMoved.Any(_ => _.MovesLeft < 0))
            return;

        UpdateUnits(unitsMoved);

        UnitsOnMap[move.To] = move.UnitIds;
        if (unitsLeft.Length == 0)
            UnitsOnMap.Remove(move.From);
        else
            UnitsOnMap[move.From] = unitsLeft;

        SomethingHappened(new GameUpdate.MoveArmy(move.From, move.To, GetUnits(unitsLeft).ToArray(), unitsMoved));
        return;
    }

    private void EndTurn(PlayerCommand.EndTurn endTurn)
    {
        if (Players.Length > 0)
        {
            var previousPlayer = CurrentPlayer;
            currentPlayerIndex = (currentPlayerIndex + 1) % Players.Length;
            var unitsToUpdate = Units.Where(_ => _.PlayerId == CurrentPlayer).ToArray();
            UpdateUnits(unitsToUpdate.Select(_ => _.NewTurn()).ToArray());
            SomethingHappened(new GameUpdate.EndTurn(previousPlayer, CurrentPlayer));
        }
    }
    private void UpdateUnits(Unit[] units)
    {
        var unitsHash = units.Select(_ => _.Id).ToHashSet();
        Units.RemoveAll(_ => unitsHash.Contains(_.Id));
        Units.AddRange(units);
    }
}

public record City(string Name, int Row, int Column);    // units to produce, row, column, defense etc.

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
public record Unit(string Id, int MovesMaximum, int MovesLeft, string PlayerId, int Strength,  UnitTypeEnum unitTypeEnum, int Upkeep)
{
    public static int GetMovementCosts(UnitTypeEnum unitType, TerrainType terrainType)
    {
        var terrainIndex = Array.IndexOf(TerrainTypes, terrainType);
        return movementSpeeds[unitType][terrainIndex];
    }

    public Unit MoveTo(TerrainType terrainType)
    {
        var costs = GetMovementCosts(unitTypeEnum, terrainType);
        if (costs > 0)
            return this with { MovesLeft = MovesLeft - costs };
        else
            return this with { MovesLeft = -1 };
    }

    public Unit NewTurn()
    {
        return this with { MovesLeft = MovesMaximum };
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
