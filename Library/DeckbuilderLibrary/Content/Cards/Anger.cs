using System.Collections.Generic;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace Content.Cards
{
    public class Anger : EnergyCard
    {
        private int DamageAmount => 6;
        public override string Name => nameof(Anger);

        public override string GetCardText(IGameEntity target = null)
        {
            return
                $"Deal {Context.GetDamageAmount(this, DamageAmount, target as ActorNode, Owner)}. Add a copy of this to your discard pile.";
        }

        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return Context.GetCurrentBattle().Graph.GetAdjacentActors((ActorNode)Owner);
        }

        public override IReadOnlyList<IGameEntity> GetAffectedEntities(IGameEntity targetCoord)
        {
            throw new System.NotImplementedException();
        }

        public override bool RequiresTarget => true;
        public override int EnergyCost => 0;

        protected override void DoPlayCard(IGameEntity target)
        {
            // Deal x damage.
            Context.TryDealDamage(this, Owner, target as ActorNode, DamageAmount);
            // Add a copy of this to your discard pile.
            Card copy = Context.CopyCard(this);
            Context.TrySendToPile(copy.Id, PileType.DiscardPile);
        }
    }
}