using System.Linq;
using Api;
using CardsAndPiles.Components;

namespace CardsAndPiles
{
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
            HandPile hand = Context.Root.GetComponentInChildren<HandPile>();
            PlayerDiscard discard = Context.Root.GetComponentInChildren<PlayerDiscard>();
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
                IEntity card = Entity.Children[Random1.SystemRandom.Next(0, Entity.Children.Count)];
                card.TrySetParent(hand.Entity);
                ((CardEvents)Context.Events).OnCardDrawn(new CardDrawnEventArgs(isHandDraw, card));
            }
        }
    }
}
