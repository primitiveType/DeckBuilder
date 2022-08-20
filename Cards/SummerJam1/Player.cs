using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Cards.Effects;
using SummerJam1.Units;
using Component = Api.Component;

namespace SummerJam1
{
    public class GrantsStrengthToAdjacent : GrantsAmount<Strength>
    {
        public override bool EveryTurn { get; }
        public override string Name => "Strength";
    }

    public class GrantsMultiAttackToAdjacent : GrantsAmount<MultiAttack>
    {
        public override bool EveryTurn { get; }
        public override string Name => "Multi-Attack";
    }

    public class GrantsArmorToAdjacent : GrantsAmount<MultiAttack>
    {
        public override bool EveryTurn { get; } = true;
        public override string Name => "Armor";
    }

    public interface IMonster
    {
    }

    public abstract class GrantsAmount<T> : EffectsAdjacentCreatures, IAmount where T : Component, IAmount, new()
    {
        protected override void ProcessAdjacentRemoved(IEntity eOldItem)
        {
            if (eOldItem.GetComponent<IMonster>() != null)
            {
                eOldItem.GetComponent<T>().Amount -= Amount;
            }
        }

        protected override void ProcessAdjacentAdded(IEntity newItem)
        {
            if (newItem.GetComponent<IMonster>() != null)
            {
                newItem.GetOrAddComponent<T>().Amount += Amount;
            }
        }

        public int Amount { get; set; }

        public override string Description
        {
            get
            {
                if (EveryTurn)
                {
                    return $"Grants {Amount} {Name} to adjacent units every turn.";
                }

                return $"Grants {Amount} {Name} to adjacent units.";
            }
        }

        public abstract string Name { get; }
    }

    public abstract class EffectsAdjacentCreatures : SummerJam1Component, IDescription
    {
        public abstract bool EveryTurn { get; }

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            if (EveryTurn)
            {
                var neighbors = Game.Battle.GetAdjacentSlots(Entity.Parent);

                foreach (IEntity neighbor in neighbors)
                {
                    foreach (IEntity neighborChild in neighbor.Children)
                    {
                        ProcessAdjacentAdded(neighborChild);
                    }
                }
            }
        }

        private List<IEntity> Tracked { get; } = new List<IEntity>();

        protected override void Initialize()
        {
            base.Initialize();
            Entity.PropertyChanged += EntityOnPropertyChanged;
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
            var neighbors = Game.Battle.GetAdjacentSlots(Entity.Parent);

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

        public abstract string Description { get; }
    }

    public class Player : SummerJam1Component, ITooltip
    {
        public int CurrentEnergy { get; set; }
        public int MaxEnergy => BattleContainer.NumEncounterSlots;


        public bool TryUseEnergy(int amount)
        {
            if (CurrentEnergy < amount)
            {
                return false;
            }

            CurrentEnergy -= amount;
            //invoke energy used.

            return true;
        }


        [OnTurnBegan]
        private void OnTurnBegan()
        {
            int energyGained = Game.Battle.EncounterSlots.Count(slot => slot.Entity.GetComponentInChildren<IGrantsEnergy>() != null);
            CurrentEnergy = energyGained;
        }

        public string Tooltip => "Energy - Cards require energy to be played. Energy is gained from slots filled with creatures.";
    }

    public class Money : SummerJam1Component, IAmount
    {
        public int Amount { get; set; }
    }

    public class DeathAddsMoneyToPlayer : SummerJam1Component
    {
        [OnEntityKilled]
        private void OnEntityKilled(object sender, EntityKilledEventArgs args)
        {
            if (args.Entity == Entity)
            {
                var amount = Entity.GetComponent<Money>().Amount;
                Game.Player.Entity.GetOrAddComponent<Money>().Amount += amount;
            }
        }
    }
}
