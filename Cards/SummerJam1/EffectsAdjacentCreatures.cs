using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1
{
    public abstract class EffectsAdjacentCreatures : SummerJam1Component, IDescription
    {
        public abstract bool EveryTurn { get; }

        private List<IEntity> Tracked { get; } = new List<IEntity>();

        public abstract string Description { get; }

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            if (EveryTurn)
            {
                List<IEntity> neighbors = Game.Battle.GetAdjacentSlots(Entity.Parent);

                foreach (IEntity neighbor in neighbors)
                {
                    foreach (IEntity neighborChild in neighbor.Children)
                    {
                        ProcessAdjacentAdded(neighborChild);
                    }
                }
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            Entity.PropertyChanged += EntityOnPropertyChanged;
            if (Entity.Parent?.GetComponent<EncounterSlotPile>() != null)
            {
                AttachToAdjacentSlots();
            }
        }

        private void EntityOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Entity.Parent))
            {
                if (Entity.Parent?.GetComponent<EncounterSlotPile>() != null)
                {
                    AttachToAdjacentSlots();
                }
                else
                {
                    RemoveFromAdjacent();
                }
            }
        }

        private void AttachToAdjacentSlots()
        {
            List<IEntity> neighbors = Game.Battle.GetAdjacentSlots(Entity.Parent);

            foreach (IEntity neighbor in neighbors)
            {
                AttachToSlot(neighbor);
            }
        }

        private void AttachToSlot(IEntity neighbor)
        {
            neighbor.Children.CollectionChanged += NeighborChildrenChanged;
            foreach (IEntity neighborChild in neighbor.Children)
            {
                ProcessAdjacentAddedInternal(neighborChild);
            }
        }

        private void DetachFromSlot(IEntity neighbor)
        {
            neighbor.Children.CollectionChanged -= NeighborChildrenChanged;
        }

        private void ProcessAdjacentAddedInternal(IEntity neighborChild)
        {
            Tracked.Add(neighborChild);
            ProcessAdjacentAdded(neighborChild);
        }

        private void NeighborChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (IEntity newItem in e.NewItems)
                {
                    ProcessAdjacentAddedInternal(newItem);
                }
            }

            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (IEntity eOldItem in e.OldItems)
                {
                    ProcessAdjacentRemovedInternal(eOldItem);
                }
            }
        }

        private void ProcessAdjacentRemovedInternal(IEntity eOldItem)
        {
            Tracked.Remove(eOldItem);
            ProcessAdjacentRemoved(eOldItem);
        }

        protected abstract void ProcessAdjacentRemoved(IEntity eOldItem);

        protected abstract void ProcessAdjacentAdded(IEntity newItem);

        
        public override void Terminate()
        {
            base.Terminate();
            RemoveFromAdjacent();
        }

        private void RemoveFromAdjacent()
        {
            foreach (IEntity entity in Tracked)
            {
                if (!EveryTurn)
                {
                    ProcessAdjacentRemoved(entity);
                }

                DetachFromSlot(entity.Parent);
            }

            Tracked.Clear();
        }
    }
}
