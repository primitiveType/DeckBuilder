using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Statuses;

namespace SummerJam1.Units.Effects
{
    public abstract class EffectsAdjacentCreatures : EnabledWhenAtTopOfEncounterSlot, IDescription
    {
        public abstract bool EveryTurn { get; }

        private List<IEntity> Tracked { get; } = new();

        public abstract string Description { get; }

        protected override void Initialize()
        {
            base.Initialize();
            Entity.PropertyChanged += EntityOnPropertyChanged;
            PropertyChanged += OnComponentPropertyChanged;
        }

        void OnComponentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Enabled))
            {
                UpdateStateOfTrackedItems();
            }
        }

        void EntityOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Entity.Parent))
            {
                UpdateStateOfTrackedItems();
            }
        }

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

        [OnBattleStarted]
        [OnEntityKilled]
        [OnEntityCreated]
        [OnUnitMoved]
        private void UpdateStateOfTrackedItems()
        {
            if (!Enabled)
            {
                RemoveFromAdjacent();
                return;
            }

            List<IEntity> currentAdj = Game.Battle.GetTopEntitiesInAdjacentSlots(Entity);

            foreach (IEntity entity in Tracked.ToList())
            {
                if (!currentAdj.Contains(entity))
                {
                    ProcessAdjacentRemovedInternal(entity);
                }
            }

            foreach (IEntity entity in currentAdj)
            {
                if (!Tracked.Contains(entity))
                {
                    ProcessAdjacentAddedInternal(entity);
                }
            }
        }

        private void ProcessAdjacentAddedInternal(IEntity neighborChild)
        {
            Tracked.Add(neighborChild);
            ProcessAdjacentAdded(neighborChild);
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
            Entity.PropertyChanged -= EntityOnPropertyChanged;
            PropertyChanged -= OnComponentPropertyChanged;
        }

        private void RemoveFromAdjacent()
        {
            foreach (IEntity entity in Tracked)
            {
                if (!EveryTurn)
                {
                    ProcessAdjacentRemoved(entity);
                }
            }

            Tracked.Clear();
        }
    }
}
