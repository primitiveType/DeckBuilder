using System;
using System.Collections.Generic;
using System.Linq;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.DataStructures;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Battles;
using DeckbuilderLibrary.Data.GameEntities.Resources;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public class ActorNode : GameEntity, IHasNeighbours<ActorNode>
    {
        private EntityReference HexGraph { get; set; } = new EntityReference();

        public HexGraph Graph => HexGraph.Entity as HexGraph;


        internal void Initialize(HexGraph graph, CubicHexCoord coord)
        {
            base.Initialize();
            HexGraph.Entity = graph;
            Coordinate = coord;
        }

        public CubicHexCoord Coordinate { get; private set; }


        private List<IGameEntity> CurrentEntities = new List<IGameEntity>();

        public void AddEntityNoEvent(IGameEntity player)
        {
            CurrentEntities.Add(player);
        }

        public IActor GetActor()
        {
            return CurrentEntities.OfType<Actor>().FirstOrDefault();
        }

        public bool TryRemove(IGameEntity entity)
        {
            return CurrentEntities.Remove(entity);
        }

        public bool TryAdd(IGameEntity entity)
        {
            if (entity is Actor)
            {
                if (CurrentEntities.OfType<IActor>().Any())
                {
                    throw new NotSupportedException("Actor tried to enter square with actor already in it!");
                    return false;
                }

                ((IInternalCoordinateProperty)entity).Coordinate = Coordinate;
            }

            CurrentEntities.Add(entity);
            return true;
        }

        public IEnumerable<ActorNode> Neighbours
        {
            get
            {
                foreach (CubicHexCoord coord in Coordinate.Neighbors())
                {
                    if(Graph.Nodes.ContainsKey(coord))
                        yield return Graph.Nodes[coord];
                }
            }
        }
    }
}