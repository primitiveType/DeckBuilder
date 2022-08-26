﻿using System.ComponentModel;
using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using PropertyChanged;

namespace SummerJam1.Cards.Effects
{
    public class DamageUnitCard : TargetSlotComponent, IEffect, IDescription
    {
        [JsonProperty] public int DamageAmount { get; private set; }
        private int FinalDamage => DamageAmount + Strength;
        [JsonProperty] public int Attacks { get; private set; } = 1;
        [JsonProperty] public bool Aoe { get; private set; }

        private int Strength { get; set; }


        [DependsOn(nameof(Strength), nameof(DamageAmount), nameof(Attacks), nameof(Aoe))]
        public string Description
        {
            get
            {
                if (Attacks == 1)
                {
                    if (Aoe)
                    {
                        return $"Deal {FinalDamage} damage to target and adjacent.";
                    }

                    return $"Deal {FinalDamage} damage.";
                }

                if (Aoe)
                {
                    return $"Deal {FinalDamage} damage to target and adjacent, {Attacks} times.";
                }

                return $"Deal {FinalDamage} damage, {Attacks} times.";
            }
        }

        public bool DoEffect(IEntity target)
        {
            for (int i = 0; i < Attacks; i++)
            {
                ITakesDamage unit = target.GetComponentInChildren<ITakesDamage>();

                if (unit == null)
                {
                    return false;
                }

                if (Aoe)
                {
                    foreach (IEntity entitiesInAdjacentSlot in Game.Battle.GetEntitiesInAdjacentSlots(target))
                    {
                        ITakesDamage health = entitiesInAdjacentSlot.GetComponent<ITakesDamage>();
                        health.TryDealDamage(DamageAmount, Game.Player.Entity);
                    }
                }

                unit.TryDealDamage(DamageAmount, Game.Player.Entity);
            }

            return true;
        }


        protected override void Initialize()
        {
            base.Initialize();
            Game.Player.Entity.GetOrAddComponent<Strength>().PropertyChanged += StrengthChanged;
            Strength = Game.Player.Entity.GetComponent<Strength>().Amount;
        }

        private void StrengthChanged(object sender, PropertyChangedEventArgs e)
        {
            Strength = Game.Player.Entity.GetComponent<Strength>().Amount;
        }

        public override void Terminate()
        {
            base.Terminate();
            Game.Player.Entity.GetComponent<Strength>().PropertyChanged -= StrengthChanged;
        }
    }
}
