using SCGraphTheory.AdjacencyList;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public class ActorEdge : EdgeBase<ActorGraphNode, ActorEdge>
    {
        public ActorEdge(ActorGraphNode @from, ActorGraphNode to) : base(@from, to)
        {
        }
    }
}