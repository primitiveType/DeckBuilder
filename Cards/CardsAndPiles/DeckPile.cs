using System.Linq;
using Api;
using CardsAndPiles.Components;

namespace CardsAndPiles
{
    public class DeckPile : Pile
    {
        public Random Random1 { get; private set; }

        private IEntity Discard { get; set; }

        private IEntity Hand { get; set; }

        public void SetHandAndDiscard(IEntity hand, IEntity discard)
        {
            Hand = hand;
            Discard = discard;
        }

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
            DrawCardInto(Hand, isHandDraw);
        }


        public void DrawCardInto(IEntity target, bool isHandDraw = false)
        {
            if (Entity.Children.Count == 0)
            {
                //shuffle discard into deck.
                foreach (IEntity discarded in Discard.Children.ToList())
                {
                    discarded.TrySetParent(Entity);
                }
            }

            if (Entity.Children.Count > 0)
            {
                IEntity card = Entity.Children[Random1.SystemRandom.Next(0, Entity.Children.Count)];
                card.TrySetParent(target);
                ((CardEvents)Context.Events).OnCardDrawn(new CardDrawnEventArgs(isHandDraw, card));
            }
        }
    }
}
