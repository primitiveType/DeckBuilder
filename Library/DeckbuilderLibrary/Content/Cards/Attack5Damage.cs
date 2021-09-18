using System;
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
    public class Attack5Damage : EnergyCard
    {
        private int DamageAmount => 5;

        public override string Name => nameof(Attack5Damage);

        public override string GetCardText(IGameEntity target = null)
        {
            return $"Deal {Context.GetDamageAmount(this, DamageAmount, target as ActorNode, Owner)} to target enemy.";
        }


        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return Context.GetCurrentBattle().Graph.GetAdjacentActors(Owner).Select(e=>e.Node).ToList();
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

        public override int EnergyCost { get; } = 0;
    }

    public class Attack5DamageAdjacent : EnergyCard
    {
        private int DamageAmount => 5;

        public override string Name => nameof(Attack5DamageAdjacent);

        public override string GetCardText(IGameEntity target = null)
        {
            return $"Deal {Context.GetDamageAmount(this, DamageAmount, target as ActorNode, Owner)} to adjacent enemies.";
        }


        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return Context.GetCurrentBattle().Graph.GetAdjacentActors(Owner);
        }

        public override IReadOnlyList<IGameEntity> GetAffectedEntities(IGameEntity targetCoord)
        {
            return Context.GetCurrentBattle().Graph.GetAdjacentActors(Owner);
        }

        public override bool RequiresTarget => false;
        private TargetingInfo AffectedTargets = new RingTargeting(1);

        protected override void DoPlayCard(IGameEntity target)
        {
            base.DoPlayCard(target);
            foreach (ActorNode node in AffectedTargets.GetNodes(Owner.Coordinate, Context))
            {
                Context.TryDealDamage(this, Owner, node, DamageAmount);
            }
        }

        public override int EnergyCost { get; } = 0;
    }

    public class Attack5DamageAdjacentAlt : EnergyCard
    {
        private int DamageAmount => 5;

        public override string Name => nameof(Attack5DamageAdjacentAlt);

        public override string GetCardText(IGameEntity target = null)
        {
            return $"Deal {Context.GetDamageAmount(this, DamageAmount, target as ActorNode, Owner)} to adjacent enemies.";
        }

        private TargetingInfo AffectedCoordinates = new RingTargeting(1);

        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            var coords = AffectedCoordinates.GetCoordinates(Owner.Coordinate);

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

        public override IReadOnlyList<IGameEntity> GetAffectedEntities(IGameEntity targetCoord)
        {
            return GetValidTargets();
        }

        public override bool RequiresTarget => false;

        protected override void DoPlayCard(IGameEntity target)
        {
            foreach (CubicHexCoord coord in AffectedCoordinates.GetCoordinates(Owner.Coordinate))
            {
                if (Context.GetCurrentBattle().Graph.TryGetNode(coord, out ActorNode targetNode))
                {
                    Context.TryDealDamage(this, Owner, targetNode, DamageAmount);
                }
            }
            // base.DoPlayCard(target);
            // if (target is ActorNode node)
            // {
            //    
            // }
            // else
            // {
            //     throw new ArgumentException("wrong type of entity! expected actor node");
            // }
        }

        public override int EnergyCost { get; } = 0;
    }

    public class RingOfFrost : EnergyCard
    {
        private int DamageAmount => 15;

        public override string Name => @"Ring of <style=""blue-ice"">Frost</style>";

        public override string GetCardText(IGameEntity target = null)
        {
            return $"Deal {Context.GetDamageAmount(this, DamageAmount, target as ActorNode, Owner)} in a ring.";
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

        public override IReadOnlyList<IGameEntity> GetAffectedEntities(IGameEntity targetCoord)
        {
            return GetValidTargets();
        }

        public override bool RequiresTarget => false;

        protected override void DoPlayCard(IGameEntity target)
        {
            base.DoPlayCard(target);
            foreach (ActorNode actorNode in GetValidTargets())
            {
                Context.TryDealDamage(this, Owner, actorNode, DamageAmount);
            }
        }

        public override int EnergyCost { get; } = 0;
    }
}