using System.Collections.Generic;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace Content.Cards
{
    public class Carnage : EnergyCard
    {
        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.CardPlayed += EventsOnCardPlayed;
            Context.Events.TurnEnded += EventsOnTurnEnded;
        }
        private void EventsOnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Id)
            {
                Context.TrySendToPile(Id, PileType.DiscardPile);
            }
        }

        private void EventsOnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            List<Card> currentHand = Context.GetCurrentBattle().Deck.HandPile.Cards;
            if (currentHand.Contains(this))
            {
                Context.TrySendToPile(Id, PileType.ExhaustPile);
            }
        }
        private int DamageAmount = 6;
        public override string Name => nameof(Carnage);
        public override string GetCardText(IGameEntity target = null)
        {
            return $"Deal {Context.GetDamageAmount(this, DamageAmount, target as IActor, Owner)}. Ethereal.";
        }
        
        public override IReadOnlyList<IActor> GetValidTargets()
        {
            return Context.GetEnemies();
        }
        public override bool RequiresTarget => true;
        public override int EnergyCost => 2;

        protected override void DoPlayCard(IActor target)
        {
            // Deal x damage.
            Context.TryDealDamage(this, Owner, target, DamageAmount);
        }
    }
}