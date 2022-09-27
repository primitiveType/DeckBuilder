using System.ComponentModel;
using System.Linq;
using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using PropertyChanged;
using SummerJam1.Piles;

namespace SummerJam1.Cards.Effects
{
    public class DamageUnitCard : TargetSlotComponent, IEffect, IDescription
    {
        [JsonProperty] public int DamageAmount { get; private set; }
        protected virtual int FinalDamage => DamageAmount + Strength;
        [JsonProperty] public int Attacks { get; private set; } = 1;
        [JsonProperty] public bool Aoe { get; private set; }

        protected int Strength { get; set; }


        [DependsOn(nameof(Strength), nameof(DamageAmount), nameof(Attacks), nameof(Aoe))]
        public virtual string Description
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

                if (Aoe)
                {
                    foreach (IEntity entitiesInAdjacentSlot in Game.Battle.GetTopEntitiesInAdjacentSlots(slot.Entity))
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
