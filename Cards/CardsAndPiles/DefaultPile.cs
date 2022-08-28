using Api;

namespace CardsAndPiles
{
    public class DefaultPile : Pile
    {
        public override bool AcceptsChild(IEntity child)
        {
            return true;
        }
    }
    
}

