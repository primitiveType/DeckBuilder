using System;
using System.Collections.Generic;
using System.Linq;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.DataStructures;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Battles;
using DeckbuilderLibrary.Data.GameEntities.Resources;
using Newtonsoft.Json;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public class ActorNode : GameEntity, IHasNeighbours<ActorNode>
    {
        [JsonIgnore] private EntityReference HexGraph { get; set; } = new EntityReference();

        [JsonIgnore]  public HexGraph Graph => HexGraph.Entity as HexGraph;

        protected override void Initialize()
        {
            base.Initialize();
        }

        internal void Initialize(HexGraph graph, CubicHexCoord coord)
        {
            base.Initialize();
            HexGraph.Entity = graph;
            Coordinate = coord;
            if (CurrentEntities == null)
            {
                CurrentEntities = new List<EntityReference>();
            }
        }

        public CubicHexCoord Coordinate { get; private set; }


        [JsonProperty] public List<EntityReference> CurrentEntities { get; set; }

        public void AddEntityNoEvent(IGameEntity player)
        {
            CurrentEntities.Add(new EntityReference(player));
        }

        public IActor GetActor()
        {
            return CurrentEntities.Select(er => er.Entity).OfType<Actor>().FirstOrDefault();
        }

        public bool TryRemove(IGameEntity entity)
        {
            var matches = CurrentEntities.Where(ce => ce.Id == entity.Id).ToList();
            var numMatches = matches.Count();
            if (numMatches == 0)
            {
                return false;
            }
            else if (numMatches > 1)
            {
                throw new NotSupportedException("Multiple matches found for entity!");
            }

            return CurrentEntities.Remove(matches[0]);
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

            CurrentEntities.Add(new EntityReference(entity));
            return true;
        }

        [JsonIgnore]
        public IEnumerable<ActorNode> Neighbours
        {
            get
            {
                foreach (CubicHexCoord coord in Coordinate.Neighbors())
                {
                    if (Graph.Nodes.ContainsKey(coord))
                        yield return Graph.Nodes[coord];
                }
            }
        }
    }
}