using System;
using System.Collections.Specialized;
using System.Linq;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Cards
{
    public class BagOfHoldingHandPile : Pile
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.Children.CollectionChanged += ChildrenOnCollectionChanged;
        }

        public override void Terminate()
        {
            base.Terminate();
            Entity.Children.CollectionChanged -= ChildrenOnCollectionChanged;
        }

        private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var eNewItem in e.NewItems)
                    {
                        ((IEntity)eNewItem).AddComponent<ZeroCostUntilPlayed>();
                        ((IEntity)eNewItem).AddComponent<CantPlayUntilEndOfTurn>();
                    }

                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var eNewItem in e.OldItems)
                    {
                        ((IEntity)eNewItem).RemoveComponent<ZeroCostUntilPlayed>();
                    }

                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool Discard()
        {
            IEntity card = Entity.Children.FirstOrDefault();
            return Discard(card);
        }

        private bool Discard(IEntity card)
        {
            if (card == null)
            {
                return false;
            }

            PlayerDiscard discard = Context.Root.GetComponentInChildren<PlayerDiscard>();
            return card.TrySetParent(discard.Entity);
        }


        public bool DiscardRandom()
        {
            IEntity card = GetRandom();
            if (card == null)
            {
                return false;
            }

            return Discard(card);
        }


        public override bool AcceptsChild(IEntity item)
        {
            Card card = item.GetComponent<Card>();

            if (card == null)
            {
                return false;
            }

            return true;
        }
    }

    public class ZeroCostUntilPlayed : SummerJam1Component, IFreePlayCard
    {
        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Entity)
            {
                Entity.RemoveComponent(this);
            }
        }
    }

    public class CantPlayUntilEndOfTurn : SummerJam1Component, ITooltip
    {
        [OnRequestPlayCard]
        private void OnRequestPlayCard(object sender, RequestPlayCardEventArgs args)
        {
            if (args.CardId == Entity)
            {
                args.Blockers.Add("That card is locked until next turn!");
            }
        }
        
        [OnTurnEnded]
        private void OnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            Entity.RemoveComponent(this);
        }

        public string Tooltip { get; } = "This card can not be played until next turn.";
    }

    public class HealthCost : SummerJam1Component, IDescription, IAmount
    {
        public int Amount { get; set; }

        public string Description => $"Lose {Amount} Health.";

        [OnRequestPlayCard]
        private void TryPlayCard(object sender, RequestPlayCardEventArgs args)
        {
            if (args.CardId == Entity)
            {
                if (Game.Player.Entity.GetComponent<Health>().Amount <= Amount)
                {
                    args.Blockers.Add(CardBlockers.NOT_ENOUGH_HEALTH); //DEBUG
                }
            }
        }

        [OnCardPlayed]
        private void CardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Entity)
            {
                if (!args.IsFree)
                {
                    Game.Player.Entity.GetComponent<Health>().DealDamage(Amount, Entity);
                }
            }
        }
    }
}