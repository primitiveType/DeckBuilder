using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Api;
using CardsAndPiles;

namespace SummerJam1.Rules
{
    public class DrawHandOnTurnBegin : SummerJam1Component
    {
        private int CardDraw => 5;

        [OnDrawPhaseBegan]
        private void OnDrawPhaseBegan()
        {
            var game = Context.Root.GetComponent<Game>();

            for (int i = 0; i < CardDraw; i++)
            {
                game.Battle.BattleDeck.DrawCard(true);
            }
        }
    }

    public class DrawEncounterHandOnTurnBegin : SummerJam1Component
    {
        private int CardDraw => 5;

        [OnDungeonPhaseStarted]
        private void OnDungeonPhaseStarted()
        {
            var game = Context.Root.GetComponent<Game>();

            for (int i = 0; i < CardDraw; i++)
            {
                game.Battle.EncounterDrawPile.DrawCard(true);
            }
        }
    }

    public class DrawEncounterHandWhenEmpty : SummerJam1Component
    {
        private int CardDraw => 5;

        protected override void Initialize()
        {
            base.Initialize();
        }

        [OnCardPlayed]
        private void OnCardPlayed()
        {
            Logging.Log("Card discarded.");
            var game = Context.Root.GetComponent<Game>();

            if (!game.Battle.EncounterHandPile.Entity.Children.Any())
            {
                Logging.Log("Hand empty.");

                for (int i = 0; i < CardDraw; i++)
                {
                    game.Battle.EncounterDrawPile.DrawCard(true);
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
