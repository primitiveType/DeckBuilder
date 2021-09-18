using System;
using System.Collections.Generic;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace Content.Cards
{
    public class Attack5Damage : EnergyCard
    {
        private int DamageAmount => 5;

        public override string Name => nameof(Attack5Damage);

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
            Context.TryDealDamage(this, Owner, target as IActor, DamageAmount);
        }

        public override int EnergyCost { get; } = 0;
    }

    public class Attack5DamageAdjacent : EnergyCard
    {
        private int DamageAmount => 5;

        public override string Name => nameof(Attack5DamageAdjacent);

        public override string GetCardText(IGameEntity target = null)
        {
            return $"Deal {Context.GetDamageAmount(this, DamageAmount, target as IActor, Owner)} to adjacent enemies.";
        }


        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return Context.GetCurrentBattle().Graph.GetAdjacentActors(Owner);
        }

        public override bool RequiresTarget => false;

        protected override void DoPlayCard(IGameEntity target)
        {
            base.DoPlayCard(target);
            foreach (var actor in Context.GetCurrentBattle().Graph.GetAdjacentActors(Owner))
            {
                Context.TryDealDamage(this, Owner, actor as IActor, DamageAmount);
            }
        }

        public override int EnergyCost { get; } = 0;
    }

    public class Attack5DamageAdjacentAlt : EnergyCard
    {
        private int DamageAmount => 5;

        public override string Name => nameof(Attack5DamageAdjacent);

        public override string GetCardText(IGameEntity target = null)
        {
            return $"Deal {Context.GetDamageAmount(this, DamageAmount, target as IActor, Owner)} to adjacent enemies.";
        }

        private TargetingInfo TargetInfo;
        private TargetingInfo AffectedTargetInfo;
        private int Range { get; set; }

        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            List<IGameEntity> neighborNodes = new List<IGameEntity>();

            var myCoord = Owner.Coordinate;
            var coords = myCoord.AreaAround(Range);

            List<IGameEntity> entities = new List<IGameEntity>();
            foreach (var coord in coords)
            {
                if (Context.GetCurrentBattle().Graph.TryGetNode(coord, out ActorNode actorNode))
                {
                    entities.Add(actorNode);
                }
            }

            return entities;
        }

        public override bool RequiresTarget => false;

        protected override void DoPlayCard(IGameEntity target)
        {
            base.DoPlayCard(target);
            if (target is ActorNode node)
            {
                foreach (CubicHexCoord coord in Info.GetAffectedCoordinates(Owner.Coordinate, node.Coordinate))
                {
                    if (Context.GetCurrentBattle().Graph.TryGetNode(coord, out ActorNode targetNode))
                    {
                        var targetActor = targetNode.GetActor();
                        Context.TryDealDamage(this, Owner, targetActor, DamageAmount);
                    }
                }
            }
            else
            {
                throw new ArgumentException("wrong type of entity! expected actor node");
            }
        }

        public override int EnergyCost { get; } = 0;
    }

    public class RingOfFrost : EnergyCard
    {
        private int DamageAmount => 15;

        public override string Name => @"Ring of <style=""blue-ice"">Frost</style>";

        public override string GetCardText(IGameEntity target = null)
        {
            return $"Deal {Context.GetDamageAmount(this, DamageAmount, target as IActor, Owner)} in a ring.";
        }


        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            List<IGameEntity> neighborNodes = new List<IGameEntity>();
            foreach (var node in Context.GetCurrentBattle().Graph.GetNodeOfActor(Owner).Neighbours)
            {
                neighborNodes.Add(node);
            }

            return neighborNodes;
        }

        public override bool RequiresTarget => false;

        protected override void DoPlayCard(IGameEntity target)
        {
            base.DoPlayCard(target);
            foreach (ActorNode actorNode in GetValidTargets())
            {
                var targetActor = actorNode.GetActor();
                if (target != null)
                {
                    Context.TryDealDamage(this, Owner, targetActor, DamageAmount);
                }
            }
        }

        public override int EnergyCost { get; } = 0;
    }
}