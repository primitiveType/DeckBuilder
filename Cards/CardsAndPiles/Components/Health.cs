using System;
using Api;

namespace CardsAndPiles.Components
{
    public class Health : CardsAndPilesComponent, ITakesDamageInternal
    {
        public int Amount { get; private set; } = 10;
        public int Max { get; private set; } = 10;

        public int TryDealDamage(int damage, IEntity source)
        {
            RequestDealDamageEventArgs args = new RequestDealDamageEventArgs(damage, source, Entity);
            Events.OnRequestDealDamage(args);
            int amount = CalculateDamage(args);
            DealDamage(amount, args.Source);
            return amount;
        }

        private int CalculateDamage(RequestDealDamageEventArgs args)
        {
            var totalMultiplier = 1f;
            foreach (var multiplier in args.Multiplier)
            {
                if (multiplier == 0f)
                {
                    totalMultiplier = multiplier;
                    break;
                }

                totalMultiplier += multiplier;
            }

            var calculated = (int)Math.Ceiling(args.Amount * totalMultiplier); //TODO: math

            foreach (var clamp in args.Clamps)
            {
                calculated = Math.Min(calculated, clamp);
            }

            return calculated;
        }

        public void DealDamage(int damage, IEntity source)
        {
            Amount -= damage;
            Events.OnDamageDealt(new DamageDealtEventArgs(Entity, source, damage));
            if (Amount <= 0)
            {
                Events.OnEntityKilled(new EntityKilledEventArgs(Entity, source));
                Entity.Destroy();
            }
        }

        [OnRequestDealDamage]
        private void OnRequestDealDamage(object sender, RequestDealDamageEventArgs args)
        {
            if (args.Target != Entity)
            {
                return;
            }
            args.Clamps.Add(Amount);
        }

        public void SetHealth(int health)
        {
            Amount = health;
        }

        public void SetMax(int max)
        {
            Max = max;
        }
    }
}