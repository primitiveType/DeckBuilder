using System;
using System.Collections.Generic;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Extensions;

namespace Content.Cards
{
    public class PommelStrike : EnergyCard
    {
        private int DamageAmount = 6;
        private int DrawAmount = 1;
        public override string Name => "Pommel Strike";

        public override string GetCardText(IGameEntity target = null)
        {           
            return
                $"Deal {Context.GetDamageAmount(this, DamageAmount, target as ActorNode, Owner)}. Draw {Context.GetDrawAmount(this, DrawAmount, target as IActor, Owner)}";
        }

        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return Context.GetEnemies();
        }

        public override IReadOnlyList<IGameEntity> GetAffectedEntities(IGameEntity targetCoord)
        {
            return new[] { targetCoord };
        }

        public override bool RequiresTarget => true;

        public override int EnergyCost => 1;

        protected override void DoPlayCard(IGameEntity target)
        {
            if (!(target is ActorNode node))
            {
                throw new NotSupportedException("Tried to play a card on the wrong target type!");
            }

            // Deal x damage.
            Context.TryDealDamage(this, Owner, node, DamageAmount);
            // Draw y cards.
            // We desperately need a way to draw a card using Context.
            // This is currently broken. It will not recycle the discard pile when there are no cards left in the draw pile.
            var deck = Context.GetCurrentBattle().Deck;
            var drawPile = deck.DrawPile;
            if (drawPile.Cards.Count > 0)
            {
                // Oh no! I'm using Deck's TrySendToPile instead of Context's!
                deck.TrySendToPile(deck.DrawPile.Cards[0], PileType.HandPile);
            }
        }
    }
}