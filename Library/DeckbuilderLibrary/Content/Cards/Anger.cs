using System.Collections.Generic;
using Data;

namespace Content.Cards
{
    public class Anger : Card
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
                // Deal x damage.
                Context.SendToPile(Id, PileType.DiscardPile);
                // Add a copy of this to your discard pile.
                Card copy = Context.CreateCopy(this);
                Context.SendToPile(copy.Id, PileType.DiscardPile);
            }
        }

        private int DamageAmount = 6;
        public override string Name => nameof(Anger);
        public override string GetCardText(IGameEntity target = null)
        {
            return $"Deal {Context.GetDamageAmount(this, DamageAmount, target as IActor, Owner)}. Add a copy of this to your discard pile.";
        }

        public override IReadOnlyList<IActor> GetValidTargets()
        {
            Context.GetEnemies();
        }

        public override bool RequiresTarget => true;
        protected override void DoPlayCard(IActor target)
        {
            Context.TryDealDamage(this, Owner, target, DamageAmount);
            
        }
    }
}