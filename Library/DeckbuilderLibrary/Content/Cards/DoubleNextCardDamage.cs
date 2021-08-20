using System;
using System.Collections.Generic;
using Data;
using Newtonsoft.Json;

namespace Content.Cards
{
    public class DoubleNextCardDamage : Card
    {
        public override string Name { get; }
        
        [JsonIgnore] private bool Activated { get; set; }
        public override string GetCardText(IGameEntity target = null)
        {
            return "Then next card you play deals double damage.";
        }

        public override IReadOnlyList<Actor> GetValidTargets()
        {
            return null;
        }

        protected override void DoPlayCard(Actor target)
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
            args.AddModifier(new DamageAmountModifier{MultiplicativeModifier = 1});
        }

        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (Activated)
            {
                Console.WriteLine("Damage doubled.");
                Activated = false;
            }
        }
    }
}