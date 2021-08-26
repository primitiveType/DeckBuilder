using System.Collections.Generic;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Resources;

namespace DeckbuilderLibrary.Data
{
    public interface IContext
    {
        int GetPlayerHealth();
        IReadOnlyList<IActor> GetEnemies();
        IBattle GetCurrentBattle();

        IActor GetActorById(int id);
        IGameEventHandler Events { get; }
        void AddEntity(IGameEntity entity);

        T CreateEntity<T>() where T : GameEntity, new();
        IDeck CreateDeck();
        IPile CreatePile();
        int GetDamageAmount(object sender, int baseDamage, IActor target, IActor owner);
        void TryDealDamage(GameEntity source, IActor owner, IActor target, int baseDamage);
        T CreateActor<T>(int health, int armor) where T : Actor, new();
        IBattle CreateBattle(IDeck deck, PlayerActor player, List<Enemy> enemies);
        T CreateIntent<T>(Actor owner) where T : Intent, new();
        T CopyCard<T>(T card) where T : Card;
        T CreateResource<T>(Actor owner, int amount) where T : Resource<T>, IResource, new();

        void TrySendToPile(int cardId, PileType pileType);

        int GetBlockAmount(object sender, int baseBlock, IActor target, IActor owner);
        int GetDrawAmount(object sender, int baseDraw, IActor target, IActor owner);
        int GetVulnerableAmount(object sender, int baseVulnerable, IActor target, IActor owner);

        void TryApplyBlock(GameEntity source, IActor owner, IActor target, int baseBlock);

        // // This particular function doesn't really work because you always draw cards 1 at a time.
        // void TryDrawCard(GameEntity source, IActor owner, IActor target, int baseDraw);
        void TryApplyVulnerable(GameEntity source, IActor owner, IActor target, int baseVulnerable);
        void EndTurn();
    }

    internal interface IInternalGameContext
    {
        List<IInternalGameEntity> ToInitialize { get; }
    }
}