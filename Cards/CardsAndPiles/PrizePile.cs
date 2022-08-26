using Api;

namespace CardsAndPiles
{
    public class PrizePile : Pile
    {
        public override bool AcceptsChild(IEntity child)
        {
            return true;
        }
    }
}
