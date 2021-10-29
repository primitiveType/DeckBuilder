using System;
using System.Collections.Generic;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Battles;
using DeckbuilderLibrary.Data.GameEntities.Resources;

namespace DeckbuilderLibrary.Data
{
    public interface IContext
    {
        int GetPlayerHealth();
        IReadOnlyList<IActor> GetEnemies();
        IBattle GetCurrentBattle();
        IGameEvents Events { get; }
        List<Card> PlayerDeck { get; }

        T CreateEntity<T>() where T : GameEntity, new();

        GameEntity CreateEntity(Type entityType);
        IBattleDeck CreateDeck();
        IPile CreatePile();
        int GetDamageAmount(object sender, int baseDamage, ActorNode target, IActor owner);
        void TryDealDamage(GameEntity source, IActor owner, ActorNode target, int baseDamage);
        T CreateActor<T>(int health, int armor) where T : Actor, new();
        IBattle StartBattle(PlayerActor player, BattleData data);
        T CreateIntent<T>(Actor owner) where T : Intent, new();
        T CopyCard<T>(T card) where T : Card;
        T CreateResource<T>(Actor owner, int amount) where T : Resource<T>, IResource, new();

        void TrySendToPile(int cardId, PileType pileType);

        void Discover(IReadOnlyList<Type> typesToDiscover, PileType destinationPile);

        int GetDrawAmount(object sender, int baseDraw, IActor target, IActor owner);

        void EndTurn();
        ActorNode CreateNode(HexGraph graph, CubicHexCoord coord);
        void EntityDestroyed(GameEntity gameEntity);
    }
}