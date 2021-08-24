using DeckbuilderLibrary.Data.Events;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public abstract class Intent : GameEntity
    {
        public abstract string GetDescription { get; }
        public int OwnerId { get; internal set; } = -1;

        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.TurnEnded += EventsOnTurnEnded;
            Context.Events.IntentChanged += EventsOnIntentChanged;
        }

        private void EventsOnIntentChanged(object sender, IntentChangedEventArgs args)
        {
            if (args.Owner.Id == OwnerId && args.Owner.Intent != this)
            {
                Context.Events.TurnEnded -= EventsOnTurnEnded;
            }
        }

        private void EventsOnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            Trigger();
        }

        protected abstract void Trigger();
    }
}