using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Piles;
using SummerJam1.Statuses;

namespace SummerJam1.Units.Effects
{
    public abstract class EffectsAdjacentCreatures : EnabledWhenAtTopOfEncounterSlot, IDescription
    {
        public abstract bool EveryTurn { get; }

        private List<IEntity> Tracked { get; } = new();

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
            this.PropertyChanged += OnComponentPropertyChanged;
            if (Entity.Parent?.GetComponent<EncounterSlotPile>() != null)
            {
                AttachToAdjacentSlots();
            }
        }

        private void OnComponentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Enabled))
            {
                UpdateState();
            }
        }

        private void EntityOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Entity.Parent))
            {
                UpdateState();
            }
        }

        private void UpdateState()
        {
            if (Enabled && Entity.Parent?.GetComponent<EncounterSlotPile>() != null)
            {
                AttachToAdjacentSlots();
            }
            else
            {
                RemoveFromAdjacent();
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
