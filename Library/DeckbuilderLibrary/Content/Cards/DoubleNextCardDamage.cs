using System;
using System.Collections.Generic;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using Newtonsoft.Json;

namespace Content.Cards
{
    public class DoubleNextCardDamage : EnergyCard
    {
        public override string Name => nameof(DoubleNextCardDamage);

        [JsonIgnore] private bool Activated { get; set; }

        public override string GetCardText(IGameEntity _ = null)
        {
            return "The next card you play deals double damage.";
        }

        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return null;
        }

        public override bool RequiresTarget => false;

        protected override void DoPlayCard(IGameEntity target)
        {
            base.DoPlayCard(target);
            Activated = true;
        }

        public override int EnergyCost => 1;

        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.CardPlayed += OnCardPlayed;
            Context.Events.DamageAmountRequested += EventsOnRequestDamageAmount;
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
                Context.TrySendToPile(Id, PileType.ExhaustPile);
            }
            else if (Activated)
            {
                Console.WriteLine("Damage doubled.");
                Activated = false;
            }
        }
    }
}