using System.Linq;
using Api;
using CardsAndPiles;

namespace Solitaire
{
    public class SolutionPile : Pile
    {
        public Suit Suit { get; internal set; }

        public override bool AcceptsChild(IEntity item)
        {
            StandardDeckCard lastChild = Entity.Children.LastOrDefault()?.GetComponent<StandardDeckCard>();
            StandardDeckCard card = item.GetComponent<StandardDeckCard>();

            if (card == null || card.Suit != Suit ||
                lastChild != null && lastChild.Number != card.Number - 1)
            {
                return false;
            }


            return true;
        }
    }
}
