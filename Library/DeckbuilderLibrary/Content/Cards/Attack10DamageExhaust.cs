using System.Collections.Generic;
using System.Linq;
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
            var battle = Context.GetCurrentBattle();
            var enemy = battle.Enemies.First();
            var slot = battle.Graph.GetAdjacentEmptyNodes(enemy);
            return slot;
        }
        

        protected override void DoPlayCard(IGameEntity target)
        {
            base.DoPlayCard(target);
            
            Context.GetCurrentBattle().Graph.MoveIntoSpace(Owner, (ActorNode)target);
        }

        public override bool RequiresTarget { get; } = true;
        public override int EnergyCost { get; } = 0;
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
            return Context.GetCurrentBattle().Graph.GetAdjacentActors(Owner);
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