namespace GodotLords.Engine;

public interface IGameUpdateHandler{
    void HandleUpdate(GameUpdate.IGameUpdate gameUpdate);
};