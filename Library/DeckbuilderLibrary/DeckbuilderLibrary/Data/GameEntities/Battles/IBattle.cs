using System.Collections.Generic;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.GameEntities.Battles
{
    public interface IBattle : IGameEntity
    {
        List<Actor> Enemies { get; }
        PlayerActor Player { get; }

        IBattleDeck Deck { get; }
        BattleGraph Graph { get; }

        // IBattleEventHandler Events { get; }
        IActor GetActorById(int id);

        void AddEnemy(Actor enemy);
        void AddEntity(IGameEntity entity);
        void TrySendToPile(int cardId, PileType pileType);
        List<IActor> GetAdjacentActors(ActorNode source);
        List<ActorNode> GetAdjacentEmptyNodes(ActorNode source);
        List<IActor> GetAdjacentActors(IActor source);
        List<ActorNode> GetAdjacentEmptyNodes(IActor source);
        void MoveIntoSpace(IActor owner, ActorNode target);
        ActorNode GetNodeOfActor(IActor actor);
    }
}