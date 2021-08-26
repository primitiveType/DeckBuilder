using System.Collections.Generic;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using Newtonsoft.Json;

namespace Content.Cards
{
    public class DealMoreDamageEachPlay : EnergyCard
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

        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return Context.GetEnemies();
        }

        public override bool RequiresTarget => true;

        protected override void DoPlayCard(IGameEntity target)
        {
            base.DoPlayCard(target);
            Context.TryDealDamage(this, Owner, target as IActor, CurrentDamage);
            TimesPlayed += 1;
        }

        private void EventsOnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Id)
            {
                Context.TrySendToPile(Id, PileType.DiscardPile);
            }
        }

        public override int EnergyCost { get; } = 1;
    }
}