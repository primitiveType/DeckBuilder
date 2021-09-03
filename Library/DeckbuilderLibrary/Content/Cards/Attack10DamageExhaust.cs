using System.Collections.Generic;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace Content.Cards
{
    public class MoveToEmptyAdjacentNode : EnergyCard
    {
        public override string Name => nameof(MoveToEmptyAdjacentNode);

        public override string GetCardText(IGameEntity target = null)
        {
            return "Move to an adjacent empty space.";
        }

        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return Context.GetCurrentBattle().GetAdjacentEmptyNodes((Actor)Owner);
        }
        

        protected override void DoPlayCard(IGameEntity target)
        {
            base.DoPlayCard(target);
            
            Context.GetCurrentBattle().MoveIntoSpace(Owner, (ActorNode)target);
        }

        public override bool RequiresTarget { get; } = true;
        public override int EnergyCost { get; } = 0;
        protected override PileType DefaultDestinationPile => PileType.DiscardPile;
    }
    public class Attack10DamageExhaust : EnergyCard
    {
        private int DamageAmount { get; } = 10;

        protected override PileType DefaultDestinationPile => PileType.ExhaustPile;
        
        public override string Name { get; } = nameof(Attack10DamageExhaust);

        public override string GetCardText(IGameEntity target = null)
        {
            return $"Deal {Context.GetDamageAmount(this, DamageAmount, target as IActor, Owner)} to target enemy.";
        }

        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return Context.GetCurrentBattle().GetAdjacentActors(Owner);
        }

        public override bool RequiresTarget => true;

        protected override void DoPlayCard(IGameEntity target)
        {
            base.DoPlayCard(target);
            Context.TryDealDamage(this, Owner, target as Actor, DamageAmount);
        }



        public override int EnergyCost => 1;
    }
}