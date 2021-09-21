using DeckbuilderLibrary.Data.DataStructures;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities.Battles;

namespace DeckbuilderLibrary.Data.GameEntities.Actors.Test
{
    public class DummyEnemy : Enemy
    {
        public int Strength => 5;
        public override int MoveSpeed { get; set; } = 0;

        protected override void Initialize()
        {
            base.Initialize();

            Context.Events.TurnStarted += OnTurnStarted;
            Context.Events.BattleEnded += OnBattleEnded;
            Context.Events.ActorDied += OnActorDied;
        }

        private void OnActorDied(object sender, ActorDiedEventArgs args)
        {
            if (args.Actor.Id == Id)
            {
                DetachEvents();
            }
        }

        private void OnBattleEnded(object sender, BattleEndedEventArgs args)
        {
            DetachEvents();
        }

        private void OnTurnStarted(object sender, TurnStartedEventArgs args)
        {
           
        }

        private void Move()
        {
           
        }

        private void DetachEvents()
        {
            Context.Events.TurnStarted -= OnTurnStarted;
            Context.Events.BattleEnded -= OnBattleEnded;
            Context.Events.ActorDied -= OnActorDied;
        }
    }
}