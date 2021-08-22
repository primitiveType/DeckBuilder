using System.Collections.Generic;
using Data;
using Newtonsoft.Json;

namespace Content.Cards
{
    public class DealMoreDamageEachPlay : Card
    {
        [JsonProperty] public int TimesPlayed { get; set; }

        private int DamageIncreasePerPlay = 1;
        private int BaseDamage = 1;

        private int CurrentDamage => (TimesPlayed * DamageIncreasePerPlay) + BaseDamage;

        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.CardPlayed += EventsOnCardPlayed;
        }

        public override string Name => nameof(DealMoreDamageEachPlay);

        public override string GetCardText(IGameEntity target = null)
        {
            return
                $"Deal {Context.GetDamageAmount(this, CurrentDamage, target as IActor, Owner)} to target enemy. Increase this card's damage by 1 for the rest of combat.";
        }

        public override IReadOnlyList<IActor> GetValidTargets()
        {
            return Context.GetEnemies();
        }

        public override bool RequiresTarget => true;

        protected override void DoPlayCard(IActor target)
        {
            Context.TryDealDamage(this, Owner, target, CurrentDamage);
            TimesPlayed += 1;
        }

        private void EventsOnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Id)
            {
                Context.TrySendToPile(Id, PileType.DiscardPile);
            }
        }
    }
}