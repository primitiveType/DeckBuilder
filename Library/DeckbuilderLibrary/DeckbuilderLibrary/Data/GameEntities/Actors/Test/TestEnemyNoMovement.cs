using DeckbuilderLibrary.Data.DataStructures;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities.Battles;

namespace DeckbuilderLibrary.Data.GameEntities.Actors.Test
{
    public class TestEnemyNoMovement : Enemy
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
            Move();
            IBattle currentBattle = Context.GetCurrentBattle();
            if (currentBattle.Graph.GetAdjacentActors(this).Contains(currentBattle.Player))
            {
                var intent = Context.CreateIntent<DamageIntent>(this);
                intent.DamageAmount = Strength;
                SetIntent(intent);
            }
        }

        private void Move()
        {
            var battle = Context.GetCurrentBattle();

            var path = new ActorNodePath(battle.Graph.GetNodeOfActor(this),
                battle.Graph.GetNodeOfActor(battle.Player));


            int moves = 0;
            foreach (var node in path)
            {
                if (moves >= MoveSpeed)
                    break;


                battle.Graph.MoveIntoSpace(this, node);
                moves++;
            }
        }

        private void DetachEvents()
        {
            Context.Events.TurnStarted -= OnTurnStarted;
            Context.Events.BattleEnded -= OnBattleEnded;
            Context.Events.ActorDied -= OnActorDied;
        }
    }
}