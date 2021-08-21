using System;
using System.Collections.Generic;
using Data;
using Newtonsoft.Json;

namespace Content.Cards
{
    public class DoubleNextCardDamage : Card
    {
        public override string Name => nameof(DoubleNextCardDamage);

        [JsonIgnore] private bool Activated { get; set; }
        public override string GetCardText(IGameEntity _ = null)
        {
            return "The next card you play deals double damage.";
        }

        public override IReadOnlyList<Actor> GetValidTargets()
        {
            return null;
        }

        public override bool RequiresTarget => false;

        protected override void DoPlayCard(Actor actor)
        {
            Activated = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.CardPlayed += OnCardPlayed;
            Context.Events.RequestDamageAmount += EventsOnRequestDamageAmount;
        }

        private void EventsOnRequestDamageAmount(object sender, RequestDamageAmountEventArgs args)
        {
            if (Activated && sender is Card)
            {
                args.AddModifier(new DamageAmountModifier { MultiplicativeModifier = 1 });
            }
        }

        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Id)
            {
                Context.SendToPile(Id, PileType.ExhaustPile);
            }
            else if (Activated)
            {
                Console.WriteLine("Damage doubled.");
                Activated = false;
            }
        }
    }
}