using System;
using System.Collections.Generic;
using DeckbuilderLibrary.Data.GameEntities;

namespace DeckbuilderLibrary.Data.DataStructures
{
    public class ActorNodeRange : Path<ActorNode>
    {
        public List<ActorNode> Nodes { get; }
        public ActorNodeRange(ActorNode start, double moveSpeed) : base(start)
        {
            Nodes = FindRange(start, moveSpeed, (actorNode, node1) => node1.GetActor() == null
                ? actorNode.Coordinate.DistanceTo(node1.Coordinate)
                : Double.MaxValue);
        }
    }
}