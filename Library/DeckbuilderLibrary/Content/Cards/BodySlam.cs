using System.Collections.Generic;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Extensions;

namespace Content.Cards
{
    public class BodySlam : EnergyCard
    {
        public override string Name => "Body Slam";
        private int DamageAmount = 0;


        protected override void DoPlayCard(IGameEntity target)
        {
            // Deal damage equal to your block.
            Context.TryDealDamage(this, Owner, target as ActorNode, DamageAmount + Owner.Armor);
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

        public override string GetCardText(IGameEntity target = null)
        {
            return $"Deal damage equal to your block.";
        }

        public override int EnergyCost => 1;
    }
}