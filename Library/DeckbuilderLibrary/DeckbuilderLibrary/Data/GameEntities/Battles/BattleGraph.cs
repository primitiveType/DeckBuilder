using SCGraphTheory.AdjacencyList;

namespace DeckbuilderLibrary.Data.GameEntities.Battles
{
    public abstract class BattleGraph : GameEntity
    {
        internal Graph<ActorGraphNode, ActorEdge> Graph { get; } = new Graph<ActorGraphNode, ActorEdge>();

        protected void ConnectNodes(ActorGraphNode a, ActorGraphNode b)
        {
            ActorEdge edgeAb = new ActorEdge(a, b);
            ActorEdge edgeBa = new ActorEdge(b, a);
            Graph.Add(edgeAb);
            Graph.Add(edgeBa);
        }
    }
}