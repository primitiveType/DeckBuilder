using System.Collections.Generic;
using DeckbuilderLibrary.Data;
using Newtonsoft.Json;

namespace Data.Cards
{
    public class Attack5Damage : Card
    {
        public override void Initialize()
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

        public override string Name { get; } = nameof(Attack5Damage);

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


    public class Attack10DamageExhaust : Card
    {
        public override void Initialize()
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

    [JsonConverter(typeof(GameEntityConverter))]
    public class DealMoreDamageEachPlay : Card
    {
        [JsonProperty] public int TimesPlayed { get; set; }

        public override void Initialize()
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