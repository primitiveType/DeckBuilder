using System.Linq;
using Api;

namespace Solitaire
{
    public class SolitairePileConstraint : Component, IParentConstraint
    {
        public bool AcceptsParent(IEntity parent)
        {
            return true;
        }

        public bool AcceptsChild(IEntity child)
        {
            StandardDeckCard card = child.GetComponent<StandardDeckCard>();

            return card != null && SuitCompatible(card.Suit) && IsNextSequence(card.Number);
        }

        private bool IsNextSequence(int number)
        {
            if (Entity.Children.Last().GetComponent<StandardDeckCard>().Number == number - 1)
            {
                return true;
            }

            return false;
        }

        private bool SuitCompatible(Suit suit)
        {
            if (Entity.Children.Count == 0)
            {
                return true;
            }

            return Entity.Children.First().GetComponent<StandardDeckCard>().Suit == suit;
        }
    }
}