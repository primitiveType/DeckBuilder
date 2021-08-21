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
            return $"Deal {Context.GetDamageAmount(this, CurrentDamage, target)} to target enemy. Increase this card's damage by 1 for the rest of combat.";
        }

        public override IReadOnlyList<Actor> GetValidTargets()
        {
            return Context.GetEnemies();
        }

        protected override void DoPlayCard(Actor target)
        {
            Context.TryDealDamage(this, target, CurrentDamage);
            TimesPlayed += 1;
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