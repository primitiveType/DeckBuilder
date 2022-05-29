using CardsAndPiles;

namespace Solitaire
{
    public class DeckPile : Pile
    {
        public override bool ReceiveItem(IPileItem item)
        {
            if ((!(item is StandardDeckCard card)) || Parent.GetComponentInParent<SolitaireGame>().GameStarted)
            {
                return false;
            }

            card.Parent.SetParent(Parent);
            return true;
        }
    }
}