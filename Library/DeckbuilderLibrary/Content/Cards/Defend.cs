using System.Collections.Generic;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Resources;

namespace Content.Cards
{
    public class Defend : EnergyCard
    {
        public override string Name => nameof(Defend);

  
   

        private int BlockAmount = 5;

        public override string GetCardText(IGameEntity target = null)
        {
            return $"Gain {BlockAmount} to target enemy.";
        }


        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return null;
        }

        public override bool RequiresTarget => false;

        protected override void DoPlayCard(IGameEntity _)
        {
            // Gain x block.
            Owner.Resources.AddResource<Armor>(BlockAmount);
        }

        public override int EnergyCost => 1;
    };
}