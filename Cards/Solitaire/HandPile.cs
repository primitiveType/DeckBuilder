using CardsAndPiles;

namespace Solitaire
{
    public class HandPile : Pile
    {
        private int MaxHandSize = 5;


        public override bool ReceiveItem(IPileItem item)
        {
            if ((!(item is StandardDeckCard card)) || Parent.Children.Count >= MaxHandSize)
            {
                return false;
            }

            card.Parent.SetParent(Parent);
            return true;
        }
    }
}