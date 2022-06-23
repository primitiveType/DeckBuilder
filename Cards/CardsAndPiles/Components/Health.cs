using System;
using System.Collections.Generic;
using Api;

namespace CardsAndPiles.Components
{
    public class Health : CardsAndPilesComponent, ITakesDamageInternal, IHealable
    {
        public int Amount { get;  set; } 
        public int Max { get;  set; } 

        public int TryDealDamage(int damage, IEntity source)
        {
            RequestDealDamageEventArgs args = new RequestDealDamageEventArgs(damage, source, Entity);
            Events.OnRequestDealDamage(args);
            int amount = CalculateDamage(args.Amount, args.Clamps, args.Multiplier);
            DealDamage(amount, args.Source);
            return amount;
        }

        private int CalculateDamage(int amount, List<int> clamps, List<float> multipliers)
        {
            var totalMultiplier = 1f;
            foreach (var multiplier in multipliers)
            {
                if (multiplier == 0f)
                {
                    totalMultiplier = multiplier;
                    break;
                }

                totalMultiplier += multiplier;
            }

            var calculated = (int)Math.Ceiling(amount * totalMultiplier); //TODO: math

            foreach (var clamp in clamps)
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
        
        public void Heal(int damage, IEntity source)
        {
            Amount += damage;
            Events.OnHealDealt(new HealDealtEventArgs(Entity, source, damage));
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
        
        [OnRequestHeal]
        private void OnRequestHeal(object sender, RequestHealEventArgs args)
        {
            if (args.Target != Entity)
            {
                return;
            }
            args.Clamps.Add(Max - Amount);
        }

        public void SetHealth(int health)
        {
            Amount = health;
        }

        public void SetMax(int max)
        {
            int diff = max - Max; 
            Max = max;
            if (Amount > max)
            {
                Amount = max;
            }

            if (diff > 0)
            {
                SetHealth(Amount + diff);
            }
        }

        public int TryHeal(int damage, IEntity source)
        {
            RequestHealEventArgs args = new RequestHealEventArgs(damage, source, Entity);
            Events.OnRequestHeal(args);
            int amount = CalculateDamage(args.Amount, args.Clamps, args.Multiplier);
            Heal(amount, args.Source);
            return amount;
        }
    }
}
