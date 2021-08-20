using System.Collections.Generic;
using Data;

namespace Content.Cards
{
    public class Attack10DamageExhaust : Card
    {
        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.CardPlayed += EventsOnCardPlayed;
        }

        public override string Name { get; } = nameof(Attack10DamageExhaust);

        public override IReadOnlyList<Actor> GetValidTargets()
        {
            return Context.GetEnemies();
        }

        public override void PlayCard(Actor target)
        {
            target.TryDealDamage(10, out int _, out int __);
            Context.Events.InvokeCardPlayed(this, new CardPlayedEventArgs(Id)); //TODO
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