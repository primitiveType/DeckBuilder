using System.Linq;
using Api;

namespace CardsAndPiles
{
    public class Card : Component, IPileItem
    {
        public bool TrySendToPile(IPile pile)
        {
            bool canMove =  Parent.GetComponents<ICardMovementConstraint>().All(constraint => constraint.CanMoveToPile(pile));
            return canMove && pile.ReceiveItem(this);
        }
    }

    public interface ICardMovementConstraint : IComponent
    {
        bool CanMoveToPile(IPile pile);
    }
}