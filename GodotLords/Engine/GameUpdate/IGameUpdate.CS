using Godot;

namespace GodotLords.Engine.GameUpdate;

public interface IGameUpdate{};
public record MoveArmy(Vector2I From, Vector2I To, Unit[] UnitsLeft, Unit[] UnitsMoved): IGameUpdate;
public record EndTurn(string PreviousPlayer, string NextPlayer): IGameUpdate;
