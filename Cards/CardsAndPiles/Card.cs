using Api;
using CardsAndPiles.Components;

namespace CardsAndPiles
{
    public abstract class Card : Component, IPileItem, IDescription
    {
        protected override void Initialize()
        {
            base.Initialize();
            ((CardEvents)Context.Events).OnCardCreated(new CardCreatedEventArgs(Entity));
        }

        public bool TryPlayCard(IEntity target)
        {
            ((CardEvents)Context.Events).OnRequestPlayCard(new RequestPlayCardEventArgs(Entity, target));

            if (!PlayCard(target))
            {
                return false;
            }

            ((CardEvents)Context.Events).OnCardPlayed(new CardPlayedEventArgs(Entity, target));
            return true;

        }

        protected abstract bool PlayCard(IEntity target);

        public virtual bool AcceptsParent(IEntity parent)
        {
            return parent.GetComponent<IPile>() != null;
        }

        
        public virtual bool AcceptsChild(IEntity child)
        {
            return true;
        }

        public abstract string Description { get; }
    }
}