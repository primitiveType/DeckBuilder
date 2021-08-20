using System.Collections.Generic;
using Data;

namespace Content.Cards
{
    public class Attack5Damage : Card
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

        public override string Name => nameof(Attack5Damage);

        public override IReadOnlyList<Actor> GetValidTargets()
        {
            return Context.GetEnemies();
        }

        public override void PlayCard(Actor target)
        {
            target.TryDealDamage(5, out int _, out int __);
            Context.Events.InvokeCardPlayed(this, new CardPlayedEventArgs(Id)); //TODO
        }
    }
}