using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Api;
using Component = Api.Component;

namespace CardsAndPiles.Components
{
    public class DescriptionComponent : Component, IDescription
    {
        public string Description { get; set; }
    }

    public class DoNothingCard : Card
    {
        protected override bool PlayCard(IEntity target)
        {
            return true;
        }
    }
    

    public abstract class Card : Component, IPileItem, IVisual
    {
        protected override void Initialize()
        {
            base.Initialize();
            ((CardEvents)Context.Events).OnCardCreated(new CardCreatedEventArgs(Entity)); //probably bad to have this here. this event does too much.
        }

        public bool TryPlayCard(IEntity target)
        {
            var args = new RequestPlayCardEventArgs(Entity, target);
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

            ((CardEvents)Context.Events).OnCardPlayed(new CardPlayedEventArgs(Entity, target, false));
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
    }
}
