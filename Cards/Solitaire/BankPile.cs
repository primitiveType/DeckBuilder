using System.Linq;
using CardsAndPiles;

namespace Solitaire
{
    public class BankPile : Pile
    {
        public override bool ReceiveItem(IPileItem item)
        {
            if ((!(item is StandardDeckCard card)) || Parent.Children.LastOrDefault()?.GetComponent<StandardDeckCard>().SuitColor == card.SuitColor)
            {
                return false;
            }

            card.Parent.SetParent(Parent);
            return true;
        }
    }
}