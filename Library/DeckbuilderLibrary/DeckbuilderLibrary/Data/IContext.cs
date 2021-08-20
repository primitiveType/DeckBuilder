using System.Collections.Generic;
using Data;

public interface IContext
{
    void SetCurrentBattle(Battle battle);
    int GetPlayerHealth();
    IReadOnlyList<Actor> GetEnemies();
    Battle GetCurrentBattle();

    Actor GetActorById(int id);
    IGameEventHandler Events { get; }
    void AddEntity(GameEntity entity);

    void SendToPile(int cardId, PileType pileType);
    T CreateEntity<T>() where T : GameEntity, new();
}

public interface IContextListener
{
    
}