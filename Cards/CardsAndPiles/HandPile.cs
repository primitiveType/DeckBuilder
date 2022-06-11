﻿using System.Linq;
using Api;

namespace CardsAndPiles
{
    public class HandPile : Pile
    {
        public bool Discard()
        {
            IEntity card = Entity.Children.FirstOrDefault();
            if (card == null)
            {
                return false;
            }

            PlayerDiscard discard = Context.Root.GetComponentInChildren<PlayerDiscard>();
            return card.TrySetParent(discard.Entity);
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
    

    public class DeckPile : Pile
    {
        public override bool AcceptsChild(IEntity child)
        {
            Card card = child.GetComponent<Card>();

            if (card == null)
            {
                return false;
            }

            return true;
        }

        public void DrawCard()
        {
            var hand = Context.Root.GetComponentInChildren<HandPile>();
            var discard = Context.Root.GetComponentInChildren<PlayerDiscard>();
            if (Entity.Children.Count == 0)
            {
                //shuffle discard into deck.
                foreach (IEntity discarded in discard.Entity.Children.ToList())
                {
                    discarded.TrySetParent(Entity);
                }
            }

            Entity.Children[0].TrySetParent(hand.Entity);
        }
    }
}