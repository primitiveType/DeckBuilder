using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Resources;

namespace DeckbuilderLibrary.Data.GameEntities.Terrain
{
    public class CoordinateEntity : GameEntity, IInternalCoordinateProperty
    {
        private CubicHexCoord m_Coordinate;
        public CubicHexCoord Coordinate => m_Coordinate;

        CubicHexCoord IInternalCoordinateProperty.Coordinate
        {
            get => m_Coordinate;
            set => SetField(ref m_Coordinate, value);
        }
    }

    public class BlockedTerrain : CoordinateEntity, IBlocksMovement
    {
    }

    public class FireTerrain : CoordinateEntity, IInternalCoordinateProperty
    {
        private int Damage => 5;

        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.TurnEnded += OnTurnEnded;
        }

        private void OnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            if (Context.GetCurrentBattle().Graph.TryGetNode(Coordinate, out ActorNode node))
            {
                Context.TryDealDamage(this, null, node, Damage);
            }
        }
    }

    public abstract class Collectible : CoordinateEntity, IInternalCoordinateProperty
    {
        private ActorNode ParentNode { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            this.AddListener(CoordinateChanged);
        }


        private void CoordinateChanged(object o, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName != nameof(Coordinate))
            {
                return;
            }

            if (ParentNode?.CurrentEntities != null)
            {
                ParentNode.CurrentEntities.CollectionChanged -= CurrentEntitiesOnCollectionChanged;
            }

            if (Context.GetCurrentBattle().Graph.TryGetNode(Coordinate, out var node))
            {
                ParentNode = node;
                node.CurrentEntities.CollectionChanged += CurrentEntitiesOnCollectionChanged;
            }
            else
            {
                ParentNode = null;
                throw new NotSupportedException("Unable to find parent node of collectible!");
            }
        }


        private void CurrentEntitiesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null)
                return;

                if (e.NewItems.Contains(Context.GetCurrentBattle().Player))
                {
                    RemoveAndDetach();
                    OnCollected();
                    Destroy();
                }
        }

        private void RemoveAndDetach()
        {
            if (ParentNode != null)
            {
                ParentNode.CurrentEntities.CollectionChanged -= CurrentEntitiesOnCollectionChanged;
                ParentNode.TryRemove(this);
            }
        }


        protected override void Terminate()
        {
            base.Terminate();
            RemoveAndDetach();
        }

        protected abstract void OnCollected();
    }
}