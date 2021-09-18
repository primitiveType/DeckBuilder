using System.Collections.Generic;
using System.Linq;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Extensions;

namespace Content.Cards
{
    public class MoveToEmptyAdjacentNode : EnergyCard
    {
        public override string Name => nameof(MoveToEmptyAdjacentNode);

        public override string GetCardText(IGameEntity target = null)
        {
            return "Move to an empty space adjacent to an enemy within range 10.";
        }

        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            var battle = Context.GetCurrentBattle();
            List<ActorNode> nodes = new List<ActorNode>();
            foreach (var enemy in Context.GetCurrentBattle().Enemies)
            {
                if (enemy.Coordinate.DistanceTo(Owner.Coordinate) > 10)
                {
                    continue;
                }

                List<ActorNode> slots = battle.Graph.GetAdjacentEmptyNodes(enemy);
                nodes = nodes.Concat(slots).ToList();
            }

            return nodes;
        }

        public override IReadOnlyList<IGameEntity> GetAffectedEntities(IGameEntity targetCoord)
        {
            return new[] { targetCoord };
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
            return $"Deal {Context.GetDamageAmount(this, DamageAmount, target as ActorNode, Owner)} to target enemy.";
        }

        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return Context.GetCurrentBattle().Graph.GetAdjacentActors(Owner).Select(a => a.Node).ToList();
        }

        public override IReadOnlyList<IGameEntity> GetAffectedEntities(IGameEntity targetCoord)
        {
            return new[] { targetCoord };
        }

        public override bool RequiresTarget => true;

        protected override void DoPlayCard(IGameEntity target)
        {
            base.DoPlayCard(target);
            Context.TryDealDamage(this, Owner, target as ActorNode, DamageAmount);
        }


        public override int EnergyCost => 1;
    }
}