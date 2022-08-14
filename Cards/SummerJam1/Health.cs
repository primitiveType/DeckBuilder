using System;
using System.Collections.Generic;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1
{
    public class Health : CardsAndPilesComponent, IHealable, IAmount, ITakesDamage
    {
        private int _amount;

        public int Amount
        {
            get => _amount;
            set
            {
                
                _amount = Math.Max(0, value);
            }
        }

        public int Max { get; set; }
        
        public bool DontDie { get; set; }

        public int TryDealDamage(int damage, IEntity source)
        {
            RequestDamageMultipliersEventArgs multipliersEventArgs = new RequestDamageMultipliersEventArgs(damage, source, Entity);
            Events.OnRequestDamageMultipliers(multipliersEventArgs);
            var multipliers = multipliersEventArgs.Multiplier;

            RequestDamageModifiersEventArgs modifiersEventArgrs = new RequestDamageModifiersEventArgs(damage, source, Entity);
            Events.OnRequestDamageModifiers(modifiersEventArgrs);

            var modifiers = modifiersEventArgrs.Modifiers;
            // Events.OnRequestDealDamage(argrs); multipliers
            // Events.OnRequestDealDamage(argrs); clamps
            // Events.OnRequestDealDamage(argrs); reduction
            int amount = CalculateDamage(multipliersEventArgs.Amount, multipliers, modifiers);
            DealDamage(amount, multipliersEventArgs.Source);
            
            return amount;
        }

        private int CalculateDamage(int amount, List<float> multipliers, List<int> reductions)
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


            foreach (int reduction in reductions)
            {
                calculated += reduction;
            }

            return calculated;
        }

        public void DealDamage(int damage, IEntity source)
        {
            var armor = Entity.GetComponent<Armor>();
            
            if (armor != null)
            {
                damage = armor.TryDealDamage(damage);
            }
            Amount -= damage;
            Events.OnDamageDealt(new DamageDealtEventArgs(Entity, source, damage));
            if (Amount <= 0 && !DontDie)
            {
                Events.OnEntityKilled(new EntityKilledEventArgs(Entity, source));
                Entity.Destroy();
            }
        }

        public void Heal(int damage, IEntity source)
        {
            damage = Math.Min(damage, Max - Amount);
            Amount += damage;
            Events.OnHealDealt(new HealDealtEventArgs(Entity, source, damage));
        }

        // [OnRequestDealDamage]
        // private void OnRequestDealDamage(object sender, RequestDealDamageEventArgs args)
        // {
        //     if (args.Target != Entity)
        //     {
        //         return;
        //     }
        //
        //     args.Clamps.Add(Amount);
        // }

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
            int amount = CalculateDamage(args.Amount, args.Multiplier, new List<int>());
            Heal(amount, args.Source);
            return amount;
        }

        
    }
}
