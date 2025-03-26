using Godot;

namespace GodotLords.Engine;


public interface ICommand;
public record MoveArmy(Vector2I From, Vector2I To): ICommand;
public record MovePartOfArmy(Vector2I From, Vector2I To, string[] UnitIds): ICommand;
