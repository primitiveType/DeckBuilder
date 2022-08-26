using System.Collections.Generic;
using System.Linq;
using Api;

namespace CardsAndPiles.Components
{
    public abstract class Card : Component, IPileItem, IVisual
    {
        public virtual bool AcceptsParent(IEntity parent)
        {
            return parent.GetComponent<IPile>() != null;
        }


        public virtual bool AcceptsChild(IEntity child)
        {
            return true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            ((CardEvents)Context.Events).OnCardCreated(new CardCreatedEventArgs(Entity)); //probably bad to have this here. this event does too much.
        }

        public bool TryPlayCard(IEntity target)
        {
            RequestPlayCardEventArgs args = new RequestPlayCardEventArgs(Entity, target);
            ((CardEvents)Context.Events).OnRequestPlayCard(args);

            if (args.Blockers.Any())
            {
                ((CardEvents)Context.Events).OnCardPlayFailed(new CardPlayFailedEventArgs(args.Blockers));
                return false;
            }

            if (!PlayCard(target))
            {
                ((CardEvents)Context.Events).OnCardPlayFailed(new CardPlayFailedEventArgs(new List<string> { "I Can't Play that right now." }));
                return false;
            }

            Entity.TrySetParent(null); //it should not be in hand while the play effects occur...
            ((CardEvents)Context.Events).OnCardPlayed(new CardPlayedEventArgs(Entity, target, false));
            return true;
        }

        protected abstract bool PlayCard(IEntity target);
    }
}
