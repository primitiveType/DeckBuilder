using System.Collections.Generic;
using Data;

namespace Content.Cards
{
    public class Strike : Card
    {
        private int DamageAmount = 6;
        public override string Name => nameof(Strike);
        public override string GetCardText(IGameEntity target = null)
        {
            return $"Deal {Context.GetDamageAmount(this, DamageAmount, target)}.";
        }

        public override IReadOnlyList<Actor> GetValidTargets()
        {
            return Context.GetEnemies();
        }

        public override bool RequiresTarget => true;
        protected override void DoPlayCard(Actor target)
        {
            Context.TryDealDamage(this, target, DamageAmount);
        }
    }
}