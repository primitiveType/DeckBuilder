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

    void TrySendToPile(int cardId, PileType pileType);
    
    T CreateEntity<T>() where T : GameEntity, new();
    IDeck CreateDeck();
    IPile CreatePile();
    Actor CreateActor<T>(int health, int armor) where T : Actor, new ();
    IBattle CreateBattle(IDeck deck, Actor player);
    T CreateIntent<T>(Actor owner) where T : Intent, new();
    T CopyCard<T>(T card) where T : Card;
    
    int GetDamageAmount(object sender, int baseDamage, IActor target, IActor owner);
    int GetBlockAmount(object sender, int baseBlock, IActor target, IActor owner);
    int GetDrawAmount(object sender, int baseDraw, IActor target, IActor owner);
    int GetVulnerableAmount(object sender, int baseVulnerable, IActor target, IActor owner);
    void TryDealDamage(GameEntity source, IActor owner,  IActor target, int baseDamage);
    void TryApplyBlock(GameEntity source, IActor owner, IActor target, int baseBlock);
    // // This particular function doesn't really work because you always draw cards 1 at a time.
    // void TryDrawCard(GameEntity source, IActor owner, IActor target, int baseDraw);
    void TryApplyVulnerable(GameEntity source, IActor owner, IActor target, int baseVulnerable);
}

public interface IContextListener
{
}