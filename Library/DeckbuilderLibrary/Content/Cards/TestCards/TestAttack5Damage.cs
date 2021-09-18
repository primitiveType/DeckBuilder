using System.Collections.Generic;
using System.Linq;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Extensions;

namespace Content.Cards.TestCards
{
    public class TestAttack5Damage : EnergyCard
    {
        private int DamageAmount => 5;

        public override string Name => nameof(TestAttack5Damage);

        public override string GetCardText(IGameEntity target = null)
        {
            return $"Deal {Context.GetDamageAmount(this, DamageAmount, target as ActorNode, Owner)} to target enemy.";
        }


        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return Context.GetCurrentBattle().Enemies.Select(e=>e.Coordinate.ToActorNode(Context)).ToList();
        }

        public override IReadOnlyList<IGameEntity> GetAffectedEntities(IGameEntity targetCoord)
        {
            return new[] { targetCoord };
        }

        public override bool RequiresTarget => true;

        protected override void DoPlayCard(IGameEntity target)
        {
            base.DoPlayCard(target);
            Context.TryDealDamage(this, Owner, target as ActorNode, 5);
        }

        public override int EnergyCost { get; } = 0;
    }

    public class TestAttack5DamageAdjacent : EnergyCard
    {
        private int DamageAmount => 5;

        public override string Name => nameof(Attack5DamageAdjacent);

        public override string GetCardText(IGameEntity target = null)
        {
            return
                $"Deal {Context.GetDamageAmount(this, DamageAmount, target as ActorNode, Owner)} to adjacent enemies.";
        }


        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return Context.GetCurrentBattle().Graph.GetAdjacentActors(Owner);
        }

        public override IReadOnlyList<IGameEntity> GetAffectedEntities(IGameEntity targetCoord)
        {
            return new[] { targetCoord };
        }

        public override bool RequiresTarget => false;

        protected override void DoPlayCard(IGameEntity target)
        {
            base.DoPlayCard(target);
            foreach (var actor in Context.GetCurrentBattle().Graph.GetAdjacentActors(Owner))
            {
                Context.TryDealDamage(this, Owner, actor as ActorNode, 5);
            }
        }

        public override int EnergyCost { get; } = 0;
    }
}