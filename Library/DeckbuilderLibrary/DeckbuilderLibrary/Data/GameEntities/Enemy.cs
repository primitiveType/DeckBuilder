using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public abstract class Enemy : Actor
    {
        public Intent Intent { get; private set; }

        public void SetIntent(Intent intent)
        {
            Intent = intent;
            Context.Events.InvokeIntentChanged(this, new IntentChangedEventArgs(this));
        }
    }
}