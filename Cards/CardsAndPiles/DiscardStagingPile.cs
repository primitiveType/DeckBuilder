using Api;

namespace CardsAndPiles
{
    public class DiscardStagingPile : Pile
    {
        public override bool AcceptsChild(IEntity child)
        {
            return true; //check if card is discardable?
        }
    }
}
