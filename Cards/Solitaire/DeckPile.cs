using Api;
using CardsAndPiles;

namespace Solitaire
{
    public class DeckPile : Pile
    {
        public override bool AcceptsChild(IEntity item)
        {
            StandardDeckCard card = item.GetComponent<StandardDeckCard>();

            return card != null && !Entity.GetComponentInParent<SolitaireGame>().GameStarted;
        }
    }
}