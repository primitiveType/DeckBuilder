using System.Collections.Generic;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public interface IPile : IGameEntity 
    {
        List<Card> Cards { get; }
        PileType PileType { get; }
    }
}