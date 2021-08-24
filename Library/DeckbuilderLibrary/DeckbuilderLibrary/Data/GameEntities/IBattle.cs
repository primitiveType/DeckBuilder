using System.Collections.Generic;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public interface IBattle : IGameEntity
    {
        List<Actor> Enemies { get; }
        PlayerActor Player { get; }

        IDeck Deck { get; }

        void AddEnemy(Actor enemy);
    }
}