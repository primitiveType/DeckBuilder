using Api;

namespace CardsAndPiles
{
    public abstract class Card : Component, IPileItem, IParentConstraint
    {
        public bool TryPlayCard(IEntity target)
        {
            if (!PlayCard(target))
            {
                return false;
            }

            Entity.GetComponentInParent<CardEventsBase>().OnCardPlayed(new CardPlayedEventArgs(Entity, target));
            return true;

        }

        protected abstract bool PlayCard(IEntity target);

        public bool AcceptsParent(IEntity parent)
        {
            return parent.GetComponent<IPile>() != null;
        }

        public virtual bool AcceptsChild(IEntity child)
        {
            return true;
        }
    }
}