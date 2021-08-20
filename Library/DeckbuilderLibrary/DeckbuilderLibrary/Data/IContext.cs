using System.Collections.Generic;
using Data;
using DeckbuilderLibrary.Data.GameEntities;

public interface IContext
{
    void SetCurrentBattle(Battle battle);
    int GetPlayerHealth();
    IReadOnlyList<Actor> GetEnemies();
    Battle GetCurrentBattle();

    Actor GetActorById(int id);
    IGameEventHandler Events { get; }
    void AddEntity(IGameEntity entity);

    void SendToPile(int cardId, PileType pileType);
    T CreateEntity<T>() where T : GameEntity, new();
    IDeck CreateDeck();
    IPile CreatePile();
}

public interface IContextListener
{
}