using System;
using System.Collections.Generic;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Resources;

namespace DeckbuilderLibrary.Data.GameEntities
{
    [BattleEntity]
    public abstract class Intent : GameEntity, IDisposable
    {
        public abstract string GetDescription { get; }
        public EntityReference<IActor> Owner { get; } = new EntityReference<IActor>();

        public abstract TargetingInfo Target { get; }

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

        public abstract List<CubicHexCoord> GetAffectedCoords();
    }
}