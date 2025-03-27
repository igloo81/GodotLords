using Godot;

namespace GodotLords.Engine.PlayerCommand;

public interface IPlayerCommand{};
public record MoveArmy(Vector2I From, Vector2I To, string[] UnitIds): IPlayerCommand;

