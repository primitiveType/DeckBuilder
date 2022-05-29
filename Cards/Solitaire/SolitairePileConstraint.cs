using System.Linq;
using Api;
using CardsAndPiles;

namespace Solitaire
{
    public class SolitairePileConstraint : Component, IPileConstraint
    {
        public bool CanReceive(Entity item)
        {
            var card = item.GetComponent<StandardDeckCard>();

            return card != null && SuitCompatible(card.Suit) && IsNextSequence(card.Number);
        }

        private bool IsNextSequence(int number)
        {
            if (Parent.Children.Last().GetComponent<StandardDeckCard>().Number == number - 1)
            {
                return true;
            }

            return false;
        }

        private bool SuitCompatible(Suit suit)
        {
            if (Parent.Children.Count == 0)
            {
                return true;
            }

            return Parent.Children.First().GetComponent<StandardDeckCard>().Suit == suit;
        }
    }
}