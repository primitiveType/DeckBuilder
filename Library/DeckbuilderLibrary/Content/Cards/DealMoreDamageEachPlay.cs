using System.Collections.Generic;
using Data;
using Newtonsoft.Json;

namespace Content.Cards
{
    public class DealMoreDamageEachPlay : Card
    {
        [JsonProperty] public int TimesPlayed { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.CardPlayed += EventsOnCardPlayed;
        }

        public override string Name { get; } = nameof(DealMoreDamageEachPlay);

        public override IReadOnlyList<Actor> GetValidTargets()
        {
            return Context.GetEnemies();
        }

        public override void PlayCard(Actor target)
        {
            target.TryDealDamage(TimesPlayed + 1, out int _, out int __);
            TimesPlayed += 1;
            Context.Events.InvokeCardPlayed(this, new CardPlayedEventArgs(Id));
        }

        private void EventsOnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Id)
            {
                Context.SendToPile(Id, PileType.DiscardPile);
            }
        }
    }
}