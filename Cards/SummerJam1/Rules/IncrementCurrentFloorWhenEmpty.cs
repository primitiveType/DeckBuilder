using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Api;
using CardsAndPiles;

namespace SummerJam1.Rules
{
    public class HandleEncounterSlotsOnTurnEnd : SummerJam1Component
    {
        [OnTurnEnded]
        private void OnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            for (int i = 0; i < BattleContainer.NumEncounterSlotsPerFloor; i++)
            {
                //slide them down.
                if (!Game.Battle.EncounterSlots[i].Entity.Children.Any())
                {
                    var child = Game.Battle.EncounterSlotsUpcoming[i].Entity.Children.FirstOrDefault();
                    if (child != null)
                    {
                        child.TrySetParent(Game.Battle.EncounterSlots[i].Entity);
                    }
                }
            }

            for (int i = BattleContainer.NumEncounterSlotsPerFloor - 1; i >= 0; i -= 1)
            {
                if (!Game.Battle.EncounterSlotsUpcoming[i].Entity.Children.Any())
                {
                    //upper slot is empty, slide to the right.

                    int sourceSlot = i - 1;
                    bool found = false;
                    while (!found)
                    {
                        if (sourceSlot < 0)
                        {
                            Game.Battle.EncounterDrawPile.DrawCardInto(Game.Battle.EncounterSlotsUpcoming[i].Entity);
                            found = true;
                        }
                        else
                        {
                            IEntity source = Game.Battle.EncounterSlotsUpcoming[sourceSlot].Entity.Children.FirstOrDefault();
                            if (source != null)
                            {
                                found = source.TrySetParent(Game.Battle.EncounterSlotsUpcoming[i].Entity);
                            }
                        }

                        sourceSlot--;
                    }
                }
            }
        }
    }

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
