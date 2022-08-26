using Api;

namespace CardsAndPiles
{
    public abstract class Pile : Component, IPile, IParentConstraint

    {
        public virtual bool AcceptsParent(IEntity parent)
        {
            return true;
        }

        public abstract bool AcceptsChild(IEntity child);
    }
}
