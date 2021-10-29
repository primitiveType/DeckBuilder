using System.Collections.Generic;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Resources;
using DeckbuilderLibrary.Extensions;
using System;

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

        public override IReadOnlyList<IGameEntity> GetAffectedEntities(IGameEntity targetCoord)
        {
            return new[] { targetCoord };
        }

        public override bool RequiresTarget => false;

        protected override void DoPlayCard(IGameEntity _)
        {
            // Gain x block.
            Owner.Resources.AddResource<Armor>(BlockAmount);
        }

        public override int EnergyCost => 1;
    }

    public class TestDiscover : EnergyCard
    {
        public override string Name => nameof(TestDiscover);

        public override string GetCardText(IGameEntity target = null)
        {
            return $"Select one card.";
        }



        public override IReadOnlyList<IGameEntity> GetValidTargets()
        {
            return null;
        }

        public override IReadOnlyList<IGameEntity> GetAffectedEntities(IGameEntity targetCoord)
        {
            return new[] { targetCoord };
        }

        public override bool RequiresTarget => false;

        protected override void DoPlayCard(IGameEntity _)
        {
              IReadOnlyList<Type> DiscoverPool  = new List<Type>
        {
            typeof(Attack10DamageExhaust), typeof(Defend)
        };

            Context.Discover(DiscoverPool, PileType.HandPile);
        
  
        }

        public override int EnergyCost => 1;
    }
}