using System.Collections.Generic;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace Content.Cards
{
    public class BodySlam : EnergyCard
    {
        public override string Name => "Body Slam";
        private int DamageAmount = 0;

       
        protected override void DoPlayCard(IGameEntity target)
        {
            // Deal damage equal to your block.
            Context.TryDealDamage(this, Owner, target as IActor, DamageAmount + Owner.Armor);
        }



        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return Context.GetEnemies();
        }

        public override bool RequiresTarget => true;

        public override string GetCardText(IGameEntity target = null)
        {
            return $"Deal damage equal to your block.";
        }

        public override int EnergyCost => 1;
    }
}