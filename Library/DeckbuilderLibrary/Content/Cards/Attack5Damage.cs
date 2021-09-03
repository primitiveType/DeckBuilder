using System;
using System.Collections.Generic;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace Content.Cards
{
    public class Attack5Damage : EnergyCard
    {
        private int DamageAmount => 5;

        protected override PileType DefaultDestinationPile => PileType.DiscardPile;

        public override string Name => nameof(Attack5Damage);

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
            Context.TryDealDamage(this, Owner, target as IActor, 5);
        }

        public override int EnergyCost { get; } = 0;
    }
    
    public class Attack5DamageAdjacent : EnergyCard
    {
        private int DamageAmount => 5;

        protected override PileType DefaultDestinationPile => PileType.DiscardPile;

        public override string Name => nameof(Attack5DamageAdjacent);

        public override string GetCardText(IGameEntity target = null)
        {
            return $"Deal {Context.GetDamageAmount(this, DamageAmount, target as IActor, Owner)} to adjacent enemies.";
        }


        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return Context.GetCurrentBattle().GetAdjacentActors(Owner);
        }

        public override bool RequiresTarget => false;

        protected override void DoPlayCard(IGameEntity target)
        {
            base.DoPlayCard(target);
            foreach (var actor in Context.GetCurrentBattle().GetAdjacentActors(Owner))
            {
                Context.TryDealDamage(this, Owner, actor as IActor, 5);
            }
        }

        public override int EnergyCost { get; } = 0;
    }

}