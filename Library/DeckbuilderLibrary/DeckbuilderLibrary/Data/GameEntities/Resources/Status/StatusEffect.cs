using DeckbuilderLibrary.Data.Events;

namespace DeckbuilderLibrary.Data.GameEntities.Resources.Status
{
    public abstract class StatusEffect<T> : Resource<T> where T : StatusEffect<T>
    {
        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.TurnEnded += OnTurnEnded;
        }

        private void OnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            TriggerEffect();
            Amount--;
        }

        protected abstract void TriggerEffect();
    }
}