using Api;

namespace CardsAndPiles
{
    public class DefaultPile : Pile
    {
        protected virtual int MaxChildren => int.MaxValue;
        public override bool AcceptsChild(IEntity child)
        {
            return Entity.Children.Count < MaxChildren;
        }
    }

}
