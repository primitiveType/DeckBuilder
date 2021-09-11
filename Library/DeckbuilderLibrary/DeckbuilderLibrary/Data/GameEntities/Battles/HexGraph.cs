using System.Collections.Generic;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.GameEntities.Battles
{
    public class HexGraph : BattleGraph
    {
        internal Dictionary<CubicHexCoord, ActorNode> Nodes { get; } = new Dictionary<CubicHexCoord, ActorNode>();
        public HexGrid Grid { get; private set; }

        public IReadOnlyDictionary<CubicHexCoord, ActorNode> GetNodes()
        {
            return Nodes;
        }

        private int radius = 10;

        protected override void Initialize()
        {
            base.Initialize();
            Grid = new HexGrid(.5f);

            for (int i = 0; i < radius; i++)
            {
                for (int j = 0; j < radius; j++)
                {
                    var coord = new AxialHexCoord(i, j).ToCubic();
                    Nodes.Add(coord, Context.CreateNode(this, coord));
                }
            }
        }

        public List<IActor> GetAdjacentActors(IActor source)
        {
            List<IActor> neighbors = new List<IActor>();
            foreach (var neighbor in source.Coordinate.Neighbors())
            {
                if (Nodes.TryGetValue(neighbor, out ActorNode node))
                {
                    var actor = node.GetActor();
                    if (actor != null)
                    {
                        neighbors.Add(actor);
                    }
                }
            }

            return neighbors;
        }

        public List<IActor> GetAdjacentActors(ActorNode source)
        {
            List<IActor> neighbors = new List<IActor>();
            // ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable method is pure.
            foreach (var neighbor in source.Coordinate.Neighbors())
            {
                if (Nodes.TryGetValue(neighbor, out ActorNode node))
                {
                    var actor = node.GetActor();
                    if (actor != null)
                    {
                        neighbors.Add(actor);
                    }
                }
            }

            return neighbors;
        }

        public List<ActorNode> GetAdjacentEmptyNodes(ActorNode source)
        {
            List<ActorNode> neighbors = new List<ActorNode>();
            // ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable method is pure.
            foreach (CubicHexCoord neighbor in source.Coordinate.Neighbors())
            {
                if (Nodes.TryGetValue(neighbor, out ActorNode node))
                {
                    var actor = node.GetActor();
                    if (actor == null)
                    {
                        neighbors.Add(Nodes[neighbor]);
                    }
                }
            }

            return neighbors;
        }

        public List<ActorNode> GetAdjacentEmptyNodes(IActor source)
        {
            List<ActorNode> neighbors = new List<ActorNode>();
            // ReSharper disable once PossiblyImpureMethodCallOnReadonlyVariable method is pure.
            foreach (CubicHexCoord neighbor in source.Coordinate.Neighbors())
            {
                if (Nodes.TryGetValue(neighbor, out ActorNode node))
                {
                    var actor = node.GetActor();
                    if (actor == null)
                    {
                        neighbors.Add(Nodes[neighbor]);
                    }
                }
            }

            return neighbors;
        }

        public ActorNode GetNodeOfActor(IActor actor)
        {
            return Nodes[actor.Coordinate];
        }

        public void MoveIntoSpace(IActor owner, ActorNode target)
        {
            if (target.GetActor() == owner)
            {
                return;//trying to move into own space, just do nothing.
            }
            var targetActor = target.GetActor();
            var prevSpace = GetNodeOfActor(owner);
            //fire event for swap? here or on properties? probably here so its one event? 
            prevSpace.TryRemove(owner);
            target.TryAdd(owner);
            prevSpace.TryAdd(targetActor);

            ((IInternalGameEvents)Context.Events).InvokeActorsSwapped(this,
                new ActorsSwappedEventArgs(owner, targetActor));
        }
    }
}