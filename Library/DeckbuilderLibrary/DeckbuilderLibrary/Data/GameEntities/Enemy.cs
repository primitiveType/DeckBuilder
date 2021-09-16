using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public abstract class Enemy : Actor
    {
        private Intent m_Intent;

        public Intent Intent
        {
            get => m_Intent;
            private set
            {
                m_Intent?.Dispose();
                m_Intent = value;
            }
        }

        public virtual int MoveSpeed { get; set; } = 1;

        public void SetIntent(Intent intent)
        {
            Intent = intent;
            ((IInternalBattleEventHandler)Context.Events).InvokeIntentChanged(this, new IntentChangedEventArgs(this));
        }
    }
}