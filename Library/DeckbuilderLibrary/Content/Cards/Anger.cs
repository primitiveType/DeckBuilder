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
                Context.TrySendToPile(Id, PileType.DiscardPile);
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
            return Context.GetEnemies();
        }

        public override bool RequiresTarget => true;
        protected override void DoPlayCard(IActor target)
        {
            // Deal x damage.
            Context.TryDealDamage(this, Owner, target, DamageAmount);
            // Add a copy of this to your discard pile.
            Card copy = Context.CopyCard(this);
            Context.TrySendToPile(copy.Id, PileType.DiscardPile);
        }
    }
}