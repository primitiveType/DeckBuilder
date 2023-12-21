using System;
using System.ComponentModel;
using System.Linq;
using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using PropertyChanged;
using SummerJam1.Piles;

namespace SummerJam1.Cards.Effects
{
    public class DamageUnitCard : TargetSlotComponent, IEffect, IDescription, ITooltip
    {
        [JsonProperty] public int DamageAmount { get; private set; }
        protected virtual int FinalDamage => DamageAmount + Strength;
        [JsonProperty] public int Attacks { get; set; } = 1;
        [JsonProperty] public bool Aoe { get; set; }
        [JsonProperty] public bool Pierce { get; set; }

        protected int Strength { get; set; }


        [DependsOn(nameof(Strength), nameof(DamageAmount), nameof(Attacks), nameof(Aoe))]
        public virtual string Description
        {
            get
            {
                string pierceString = Pierce ? "Pierce." : "";
                if (Attacks == 1)
                {
                    if (Aoe)
                    {
                        return $"Deal {FinalDamage} damage to target and adjacent. {pierceString}";
                    }

                    return $"Deal {FinalDamage} damage. {pierceString}";
                }

                if (Aoe)
                {
                    return $"Deal {FinalDamage} damage to target and adjacent, {Attacks} times. {pierceString}";
                }

                return $"Deal {FinalDamage} damage, {Attacks} times. {pierceString}";
            }
        }

        public virtual bool DoEffect(IEntity target)
        {
            EncounterSlotPile slot = target.GetComponentInSelfOrParent<EncounterSlotPile>();

            for (int i = 0; i < Attacks; i++)
            {
                ITakesDamage unit = slot.Entity.Children.LastOrDefault()?.GetComponentInChildren<ITakesDamage>();

                if (unit == null)
                {
                    return false;
                }

                if (Pierce)
                {
                    foreach (IEntity entity in slot.Entity.Children.Where(card => card.GetComponent<FaceDown>() == null).ToList())//create snapshot
                    {
                        ITakesDamage backUnit = entity.GetComponentInChildren<ITakesDamage>();
                        backUnit.TryDealDamage(DamageAmount, Game.Player.Entity);
                    }
                }


                if (Aoe)
                {
                    throw new NotSupportedException();
                    // foreach (IEntity entitiesInAdjacentSlot in Game.Battle.GetTopEntitiesInAdjacentSlots(slot.Entity).ToList())//create snapshot.
                    // {
                    //     ITakesDamage health = entitiesInAdjacentSlot.GetComponent<ITakesDamage>();
                    //     health.TryDealDamage(DamageAmount, Game.Player.Entity);
                    // }
                }

                if (!Pierce)
                {
                    unit.TryDealDamage(DamageAmount, Game.Player.Entity);
                }
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

        public string Tooltip => Pierce ? PierceTooltip.PIERCE_TOOLTIP : null;
    }
}
