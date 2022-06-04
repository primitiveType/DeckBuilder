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
            Game = Entity.GetComponentInParent<SolitaireGame>();
        }

        public override bool AcceptsChild(IEntity item)
        {
            if (!Game.GameStarted)
            {
                foreach (IEntity parentChild in Entity.Children)
                {
                    parentChild.GetComponent<StandardDeckCard>().IsFaceDown = true;
                }

                return true;
            }

            StandardDeckCard card = item.GetComponent<StandardDeckCard>();
            return card != null && Entity.Children.LastOrDefault()?.GetComponent<StandardDeckCard>().SuitColor !=
                card.SuitColor;
        }
    }

    public class SolutionPile : Pile
    {
        public Suit Suit { get; internal set; }

        public override bool AcceptsChild(IEntity item)
        {
            StandardDeckCard lastChild = Entity.Children.LastOrDefault()?.GetComponent<StandardDeckCard>();
            StandardDeckCard card = item.GetComponent<StandardDeckCard>();

            if ((card == null) || card.Suit != Suit ||
                lastChild != null && lastChild.Number != card.Number - 1)
            {
                return false;
            }


            return true;
        }
    }
}