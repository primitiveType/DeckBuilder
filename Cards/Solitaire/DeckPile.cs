using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace Solitaire
{
    public class DeckPile : Pile
    {
        public override bool AcceptsChild(IEntity item)
        {
            Card card = item.GetComponent<Card>();

            return card != null && !Entity.GetComponentInParent<SolitaireGame>().GameStarted;
        }
    }
}