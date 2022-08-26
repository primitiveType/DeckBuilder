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

            int index = Context.Root.GetComponent<Random>().SystemRandom.Next(0, Entity.Children.Count);
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
}
