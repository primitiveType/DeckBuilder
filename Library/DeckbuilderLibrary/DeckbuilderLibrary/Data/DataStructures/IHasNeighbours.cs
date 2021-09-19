using System.Collections.Generic;

namespace DeckbuilderLibrary.Data.DataStructures
{
    public interface IHasNeighbours<N>
    {
        IEnumerable<N> Neighbours { get; }
        IEnumerable<N> TraversableNeighbours { get; }
    }
}