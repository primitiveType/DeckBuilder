using System.Collections.Generic;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public interface IBattle : IGameEntity
    {
        List<Actor> Enemies { get; }
        PlayerActor Player { get; }

        IBattleDeck Deck { get; }

        // IBattleEventHandler Events { get; }
        IActor GetActorById(int id);

        void AddEnemy(Actor enemy);
        void AddEntity(IGameEntity entity);
        void TrySendToPile(int cardId, PileType pileType);
    }
}