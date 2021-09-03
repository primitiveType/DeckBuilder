using SCGraphTheory.AdjacencyList;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public class ActorGraphNode : NodeBase<ActorGraphNode, ActorEdge>
    {
        public ActorNode
            ActorNode { get; set; } //gotta figure out events and stuff, and setting these up without firing  them.

        public ActorGraphNode()
        {
        }

        public ActorGraphNode(ActorNode actorNode)
        {
            ActorNode = actorNode;
        }
    }
}