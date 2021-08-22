using System.Collections.Generic;
using Data;

namespace Content.Cards
{
    public class Strike : Card
    {
        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.CardPlayed += EventsOnCardPlayed;
        }
        private void EventsOnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Id)
            {
                Context.SendToPile(Id, PileType.DiscardPile);
            }
        }
        
        private int DamageAmount = 6;
        public override string Name => nameof(Strike);
        public override string GetCardText(IGameEntity target = null)
        {
            return $"Deal {Context.GetDamageAmount(this, DamageAmount, target as IActor, Owner)}.";
        }

        public override IReadOnlyList<IActor> GetValidTargets()
        {
            return Context.GetEnemies();
        }

        public override bool RequiresTarget => true;

        protected override void DoPlayCard(IActor target)
        {
            Context.TryDealDamage(this, Owner, target, DamageAmount);
        }
    }
}