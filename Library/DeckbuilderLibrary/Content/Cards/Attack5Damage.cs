using System.Collections.Generic;
using Data;

namespace Content.Cards
{
    public class Attack5Damage : Card
    {
        private int DamageAmount => 5;

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

        public override string Name => nameof(Attack5Damage);

        public override string GetCardText(IGameEntity target = null)
        {
            return $"Deal {Context.GetDamageAmount(this, DamageAmount, target as IActor, Owner)} to target enemy.";
        }


        public override IReadOnlyList<IActor> GetValidTargets()
        {
            return Context.GetEnemies();
        }

        public override bool RequiresTarget => true;

        protected override void DoPlayCard(IActor target)
        {
            Context.TryDealDamage(this, Owner, target, 5);
        }
    }
}