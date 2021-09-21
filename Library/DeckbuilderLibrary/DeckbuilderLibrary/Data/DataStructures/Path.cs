using System;
using System.Collections;
using System.Collections.Generic;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.DataStructures
{
    public class Path<Node> : IEnumerable<Node>
    {
        public Node LastStep { get; private set; }
        public Path<Node> PreviousSteps { get; private set; }
        public double TotalCost { get; private set; }

        private Path(Node lastStep, Path<Node> previousSteps, double totalCost)
        {
            LastStep = lastStep;
            PreviousSteps = previousSteps;
            TotalCost = totalCost;
        }

        public Path(Node start) : this(start, null, 0)
        {
        }

        public Path<Node> AddStep(Node step, double stepCost)
        {
            return new Path<Node>(step, this, TotalCost + stepCost);
        }

        public virtual IEnumerator<Node> GetEnumerator()
        {
            for (Path<Node> p = this; p != null; p = p.PreviousSteps)
                yield return p.LastStep;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        public static Path<Node> FindPath<Node>(
            Node start,
            Node destination,
            Func<Node, Node, double> distance,
            Func<Node, double> estimate)
            where Node : IHasNeighbours<Node>
        {
            var closed = new HashSet<Node>();
            var queue = new PriorityQueue<double, Path<Node>>();
            queue.Enqueue(0, new Path<Node>(start));
            while (!queue.IsEmpty)
            {
                var path = queue.Dequeue();
                if (closed.Contains(path.LastStep))
                    continue;
                if (path.LastStep.Equals(destination))
                    return path;
                closed.Add(path.LastStep);
                foreach (Node n in path.LastStep.Neighbours)
                {
                    double d = distance(path.LastStep, n);
                    var newPath = path.AddStep(n, d);
                    queue.Enqueue(newPath.TotalCost + estimate(n), newPath);
                }
            }

            return null;
        }

        public static List<Node> FindRange<Node>(
            Node start,
            double distanceBudget,
            Func<Node, Node, double> distance)
            where Node : IHasNeighbours<Node>
        {
            List<Node> nodes = new List<Node>();

            AddRange(nodes, start, distanceBudget, distance);

            return nodes;
        }

        private static void AddRange<TNode>(
            List<TNode> appendTo,
            TNode start,
            double distanceBudget,
            Func<TNode, TNode, double> distance)
            where TNode : IHasNeighbours<TNode>
        {
            if (distanceBudget <= 0)
            {
                return;
            }

            foreach (var node in start.Neighbours)
            {
                var distanceRemaining = distanceBudget - distance(start, node);
                if (distanceRemaining >= 0)
                {
                    appendTo.Add(node);
                    AddRange<TNode>(appendTo, node, distanceRemaining, distance);
                }
            }
        }
    }
}