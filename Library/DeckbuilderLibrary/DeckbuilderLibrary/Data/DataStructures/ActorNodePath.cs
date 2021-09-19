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

        public ActorNodePath(ActorNode start, ActorNode dest, double maxDistance, bool includeStart = false,
            bool includeDestination = false) :
            base(start)
        {
            IncludeStart = includeStart;
            IncludeDestination = includeDestination;
            Start = start;
            Destination = dest;
            MaxDistance = maxDistance;
            Path = FindPath(start, dest, 100);
        }

        private ActorNode Start { get; }

        private ActorNode Destination { get; }
        
        private double MaxDistance { get; set; }

        private static Path<ActorNode> FindPath(ActorNode sourceNode, ActorNode destNode, double maxDistance)
        {
            var path = FindPath(sourceNode, destNode,
                Distance,
                actorNode => actorNode.Coordinate.DistanceTo(destNode.Coordinate));
            return path;
        }

        private static double Distance(ActorNode actorNode, ActorNode node1)
        {
            return node1.IsBlocked()
                ? Double.MaxValue
                : actorNode.Coordinate.DistanceTo(node1.Coordinate);
        }


        public override IEnumerator<ActorNode> GetEnumerator()
        {
            if (Path == null)
            {
                yield break;
            }

            double currentDistance = 0;
            var lastNode = Start;

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

                currentDistance += Distance(lastNode, node);
                if (currentDistance > MaxDistance)
                {
                    yield break;
                }
                yield return node;
            }
        }
    }
}