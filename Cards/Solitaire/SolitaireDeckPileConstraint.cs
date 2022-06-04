using Api;
using CardsAndPiles;

namespace Solitaire
{
    public class SolitaireDeckPileConstraint : Component, IPileConstraint
    {
        public bool CanReceive(IEntity itemView)
        {
            return !Parent.GetComponentInParent<SolitaireGame>().GameStarted;
        }
    }
}