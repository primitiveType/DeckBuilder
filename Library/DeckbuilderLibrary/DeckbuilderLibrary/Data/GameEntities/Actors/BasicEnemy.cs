using DeckbuilderLibrary.Data.GameEntities.Battles;

namespace DeckbuilderLibrary.Data.GameEntities.Actors
{
    public class BasicEnemy : Enemy
    {
        public int Strength => 5;

        protected override void Initialize()
        {
            base.Initialize();

            Context.Events.TurnStarted += OnTurnStarted;
        }

        private void OnTurnStarted(object sender, TurnStartedEventArgs args)
        {
            IBattle currentBattle = Context.GetCurrentBattle();
            if (currentBattle.GetAdjacentActors(this).Contains(currentBattle.Player))
            {
                var intent = Context.CreateIntent<DamageIntent>(this);
                intent.DamageAmount = Strength;
                SetIntent(intent);
            }
            else
            {
                //want to do movement intent, but there's some questions I have there
                
                var intent = Context.CreateIntent<MoveIntent>(this);
                SetIntent(intent);
            }
        }
    }
}