using System.Linq;
using Api;
using CardsAndPiles;

namespace Solitaire
{
    public class BankPile : Pile
    {
        private SolitaireGame Game { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            Game = Parent.GetComponentInParent<SolitaireGame>();
        }

        public override bool ReceiveItem(IPileItem item)
        {
            if (!Game.GameStarted)
            {
                foreach (Entity parentChild in Parent.Children)
                {
                    parentChild.GetComponent<StandardDeckCard>().IsFaceDown = true;
                }

                return true;
            }

            if (!(item is StandardDeckCard card) ||
                Parent.Children.LastOrDefault()?.GetComponent<StandardDeckCard>().SuitColor == card.SuitColor)
            {
                return false;
            }

            card.Parent.SetParent(Parent);
            return true;
        }
    }

    public class SolutionPile : Pile
    {
        public Suit Suit { get; internal set; }

        public override bool ReceiveItem(IPileItem item)
        {
            StandardDeckCard lastChild = Parent.Children.LastOrDefault()?.GetComponent<StandardDeckCard>();

            if (!(item is StandardDeckCard card) || card.Suit != Suit ||
                lastChild != null && lastChild.Number != card.Number - 1)
            {
                return false;
            }

            card.Parent.SetParent(Parent);
            return true;
        }
    }
}