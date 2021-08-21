using System.Collections.Generic;
using Data;
using DeckbuilderLibrary.Data.GameEntities;

public interface IContext
{
    void SetCurrentBattle(IBattle battle);
    int GetPlayerHealth();
    IReadOnlyList<IActor> GetEnemies();
    IBattle GetCurrentBattle();

    IActor GetActorById(int id);
    IGameEventHandler Events { get; }
    void AddEntity(IGameEntity entity);

    void SendToPile(int cardId, PileType pileType);
    T CreateEntity<T>() where T : GameEntity, new();
    IDeck CreateDeck();
    IPile CreatePile();
    int GetDamageAmount(object sender, int baseDamage, IGameEntity target);
    void TryDealDamage(GameEntity source, IActor target, int baseDamage);
    Actor CreateActor<T>(int health, int armor) where T : Actor;
    IBattle CreateBattle(IDeck deck, Actor player);
}

public interface IContextListener
{
}