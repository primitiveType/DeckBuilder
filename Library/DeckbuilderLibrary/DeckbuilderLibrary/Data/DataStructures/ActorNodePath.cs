using System;
using System.Collections.Generic;
using System.Linq;
using DeckbuilderLibrary.Data.GameEntities;

namespace DeckbuilderLibrary.Data.DataStructures
{
    public class ActorNodePath : Path<ActorNode>
    {
        private Path<ActorNode> Path { get; }
        private bool IncludeDestination { get; }
        private bool IncludeStart { get; }

        public ActorNodePath(ActorNode start, ActorNode dest, bool includeStart = false, bool includeDestination = false) :
            base(start)
        {
            IncludeStart = includeStart;
            IncludeDestination = includeDestination;
            Start = start;
            Destination = dest;
            Path = FindPath(start, dest);
        }

        private ActorNode Start { get; }

        private ActorNode Destination { get; }

        private static Path<ActorNode> FindPath(ActorNode sourceNode, ActorNode destNode)
        {
            var path = FindPath(sourceNode, destNode,
                (actorNode, node1) => node1.GetActor() == null
                    ? actorNode.Coordinate.DistanceTo(node1.Coordinate)
                    : Double.MaxValue,
                actorNode => actorNode.Coordinate.DistanceTo(destNode.Coordinate));
            return path;
        }

        public override IEnumerator<ActorNode> GetEnumerator()
        {
            foreach (var node in Path.Reverse())
            {
                if (!IncludeDestination && node == Destination)
                {
                    continue;
                }

                if (!IncludeStart && node == Start)
                {
                    continue;
                }

                yield return node;
            }
        }
    }
}