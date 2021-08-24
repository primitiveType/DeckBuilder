using System.Collections.Generic;
using System;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace Content.Cards
{
    public class SwordBoomerang : EnergyCard
    {
        protected override void Initialize()
        {
            base.Initialize();
            Context.Events.CardPlayed += EventsOnCardPlayed;
        }
        private void EventsOnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Id)
            {
                Context.TrySendToPile(Id, PileType.DiscardPile);
            }
        }
        private int DamageAmount = 6;
        private int RepeatAmount = 4;
        public override string Name => "Sword Boomerang";
        public override string GetCardText(IGameEntity target = null)
        {
            return $"Deal {Context.GetDamageAmount(this, DamageAmount, target as IActor, Owner)} to a random enemy {RepeatAmount}.";
        }
        
        public override IReadOnlyList<IActor> GetValidTargets()
        {
            return Context.GetEnemies();
        }
        public override bool RequiresTarget => false;

        public override int EnergyCost => 1;

        protected override void DoPlayCard(IActor _)
        {
            // Deal x damage to y random enemies.
            var random = new Random();
            for (var i = 0; i < RepeatAmount; i++)
            {
                var enemies = GetValidTargets();
                var enemyCount = enemies.Count;
                if (enemyCount == 0)
                {
                    return;
                }
                var randomIndex = random.Next(enemies.Count);
                var target = enemies[randomIndex];
                Context.TryDealDamage(this, Owner, target, DamageAmount);
            }
        }
    }
}