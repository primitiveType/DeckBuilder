using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Annotations;
using DeckbuilderLibrary.Data.DataStructures;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Battles;
using DeckbuilderLibrary.Data.GameEntities.Resources;
using DeckbuilderLibrary.Data.GameEntities.Terrain;
using Newtonsoft.Json;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public class ActorNode : CoordinateEntity, IHasNeighbours<ActorNode>
    {
        [JsonIgnore] private EntityReference<HexGraph> HexGraph { get; set; } = new EntityReference<HexGraph>();

        [JsonIgnore] public HexGraph Graph => HexGraph.Entity as HexGraph;


        internal void Initialize(HexGraph graph, CubicHexCoord coord)
        {
            base.Initialize();
            HexGraph.Entity = graph;
            ((IInternalCoordinateProperty)this).Coordinate = coord;
            if (CurrentEntities == null)
            {
                CurrentEntities = new ObservableCollection<EntityReference<IGameEntity>>();
            }
        }


        [JsonProperty] public ObservableCollection<EntityReference<IGameEntity>> CurrentEntities { get; set; }

        [PublicAPI]
        public void AddEntityNoEvent(IGameEntity player)
        {
            TryAdd(player);
            // CurrentEntities.Add(new EntityReference<IGameEntity>(player));
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
            if (entity == null)
            {
                return false;
            }

            if (entity is IBlocksMovement)
            {
                if (CurrentEntities.OfType<IBlocksMovement>().Any())
                {
                    throw new NotSupportedException(
                        "IBlocksMovement tried to enter square with IBlocksMovement already in it!");
                    return false;
                }
            }

            if (entity is IInternalCoordinateProperty coordinateProperty)
            {
                coordinateProperty.Coordinate = Coordinate;
            }

            CurrentEntities.Add(new EntityReference<IGameEntity>(entity));
            return true;
        }

        [JsonIgnore]
        public IEnumerable<ActorNode> Neighbours
        {
            get
            {
                foreach (CubicHexCoord coord in Coordinate.Neighbors())
                {
                    if (Graph.TryGetNode(coord, out var node))
                    {
                        yield return node;
                    }
                }
            }
        }

        public IEnumerable<ActorNode> TraversableNeighbours
        {
            get
            {
                foreach (CubicHexCoord coord in Coordinate.Neighbors())
                {
                    if (!Graph.TryGetNode(coord, out var node))
                    {
                        continue;
                    }

                    if (node.IsBlocked())
                    {
                        continue;
                    }

                    yield return node;
                }
            }
        }

        public bool IsBlocked()
        {
            return CurrentEntities.Any(entity => entity.Entity is IBlocksMovement);
        }
    }
}