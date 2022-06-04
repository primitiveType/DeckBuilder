using Api;
using CardsAndPiles;

namespace Solitaire
{
    public class HandPile : Pile
    {
        private int MaxHandSize = 52;


        public override bool AcceptsChild(IEntity item)
        {
            StandardDeckCard card = item.GetComponent<StandardDeckCard>();

            if (card != null || Entity.Children.Count >= MaxHandSize)
            {
                return false;
            }

            return true;
        }
    }
}