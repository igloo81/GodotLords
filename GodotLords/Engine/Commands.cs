using Godot;

namespace GodotLords.Engine;

public interface IGameCommand{};
public record MoveArmy(Vector2I From, Vector2I To): IGameCommand;
public record MovePartOfArmy(Vector2I From, Vector2I To, string[] UnitIds): IGameCommand;
