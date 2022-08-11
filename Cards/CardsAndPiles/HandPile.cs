using System.Linq;
using Api;
using CardsAndPiles.Components;

namespace CardsAndPiles
{
    public class HandPile : Pile
    {
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
            if (Entity.Children.Count == 0)
            {
                return false;
            }

            var index = Context.Root.GetComponent<Random>().SystemRandom.Next(0, Entity.Children.Count);
            IEntity card = Entity.Children.ElementAt(index);

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
    
    public class CardPrefabPile : Pile
    {
        public override bool AcceptsChild(IEntity child)
        {
            return true;
        }
    }

    public class PrizePile : Pile
    {
        public override bool AcceptsChild(IEntity child)
        {
            return true;
        }
    }

    public class ObjectivesPile : Pile
    {
        public override bool AcceptsChild(IEntity child)
        {
            return true;
        }
    }


    public class DeckPile : Pile
    {
        public Random Random1 { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();
            Random1 = Context.Root.GetComponent<Random>();
        }

        public override bool AcceptsChild(IEntity child)
        {
            Card card = child.GetComponent<Card>();

            if (card == null)
            {
                return false;
            }

            return true;
        }

        public void DrawCard(bool isHandDraw = false)
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

            if (Entity.Children.Count > 0)
            {
                Entity.Children[Random1.SystemRandom.Next(0, Entity.Children.Count)].TrySetParent(hand.Entity);
                ((CardsAndPiles.CardEvents)(Context.Events)).OnCardDrawn(new CardDrawnEventArgs(isHandDraw));
            }
        }
    }
    
    public class EncounterDeckPile : Pile
    {
        public Random Random1 { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();
            Random1 = Context.Root.GetComponent<Random>();
        }

        public override bool AcceptsChild(IEntity child)
        {
            Card card = child.GetComponent<Card>();

            if (card == null)
            {
                return false;
            }

            return true;
        }

        public void DrawCard(bool isHandDraw = false)
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

            if (Entity.Children.Count > 0)
            {
                Entity.Children[Random1.SystemRandom.Next(0, Entity.Children.Count)].TrySetParent(hand.Entity);
                ((CardsAndPiles.CardEvents)(Context.Events)).OnCardDrawn(new CardDrawnEventArgs(isHandDraw));
            }
        }
    }
}
