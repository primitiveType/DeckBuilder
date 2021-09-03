using System;
using DeckbuilderLibrary.Data.Events;

namespace DeckbuilderLibrary.Data.GameEntities
{
    [BattleEntity]
    public abstract class Intent : GameEntity, IDisposable
    {
        public abstract string GetDescription { get; }
        public int OwnerId { get; internal set; } = -1;

        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.TurnEnded += EventsOnTurnEnded;
            Context.Events.BattleEnded += OnBattleEnded;
        }

        protected virtual void OnBattleEnded(object sender, BattleEndedEventArgs args)
        {
            Dispose();
        }

        private void EventsOnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            Trigger();
        }

        protected abstract void Trigger();

        public void Dispose()
        {
            Context.Events.TurnEnded -= EventsOnTurnEnded;
            Context.Events.BattleEnded -= OnBattleEnded;
        }
    }
}