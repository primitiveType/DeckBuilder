using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using CardsAndPiles;

namespace SummerJam1.Rules
{
    public class IncrementCurrentFloorWhenEmpty : SummerJam1Component
    {
        private List<Pile> Slots { get; } = new List<Pile>();

        protected override void Initialize()
        {
            base.Initialize();
            Game.Battle.PropertyChanged += BattleOnPropertyChanged;
            AttachToEncounterSlots();
        }

        private void AttachToEncounterSlots()
        {
            foreach (Pile battleEncounterSlot in Game.Battle.EncounterSlots)
            {
                Slots.Add(battleEncounterSlot);
                battleEncounterSlot.Entity.Children.CollectionChanged += ChildrenOnCollectionChanged;
            }
        }

        private void BattleOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BattleContainer.CurrentFloor))
            {
                ReAttach();
            }
        }

        private void ReAttach()
        {
            DetachFromEncounterSlots();
            AttachToEncounterSlots();
        }

        private void DetachFromEncounterSlots()
        {
            foreach (Pile slot in Slots)
            {
                slot.Entity.Children.CollectionChanged -= ChildrenOnCollectionChanged;
            }

            Slots.Clear();
        }

        private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // TryMoveToNextFloor();
        }

        [OnTurnBegan]
        private void TryMoveToNextFloor()
        {
            if (Game.Battle.GetEmptySlots().Count() != BattleContainer.NumEncounterSlotsPerFloor)
            {
                return;
            }

            Game.Battle.MoveToNextFloor();
        }

        public override void Terminate()
        {
            base.Terminate();
            DetachFromEncounterSlots();
        }
    }
}
