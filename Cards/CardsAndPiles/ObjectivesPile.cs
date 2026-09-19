using Api;

namespace CardsAndPiles
{
    public class ObjectivesPile : Pile
    {
        public override bool AcceptsChild(IEntity child)
        {
            return true;
        }
    }
}
