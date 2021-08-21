using System.Collections.Generic;
using Data;

namespace Content.Cards
{
    public class Attack10DamageExhaust : Card
    {
        private int DamageAmount { get; } = 10;

        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.CardPlayed += EventsOnCardPlayed;
        }

        public override string Name { get; } = nameof(Attack10DamageExhaust);

        public override string GetCardText(IGameEntity target = null)
        {
            return $"Deal {Context.GetDamageAmount(this, DamageAmount, target)} to target enemy.";
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

        private void EventsOnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Id)
            {
                Context.SendToPile(Id, PileType.ExhaustPile);
            }
        }
    }
}