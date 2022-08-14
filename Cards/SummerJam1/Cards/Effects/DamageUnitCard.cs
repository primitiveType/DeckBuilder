using System;
using System.ComponentModel;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1.Cards.Effects
{
    public class TargetHealthComponent : SummerJam1Component
    {
        [OnRequestPlayCard]
        private void OnRequestPlayCard(object sender, RequestPlayCardEventArgs args)
        {
            if (args.CardId != Entity)
            {
                return;
            }

            var target = args.Target.GetComponentInChildren<Health>();

            if (target == null)
            {
                args.Blockers.Add(CardBlockers.INVALID_TARGET);
            }
        }
    }

    public class DamageUnitEqualToStealth : SummerJam1Component, IEffect
    {
        public bool DoEffect(IEntity target)
        {
            ITakesDamage unit = target.GetComponentInChildren<ITakesDamage>();

            if (unit == null)
            {
                return false;
            }


            unit.TryDealDamage(Game.Player.Entity.GetComponent<Stealth>().Amount, Entity);
            return true;
        }
    }

    public class StealthRequirement : SummerJam1Component, IDescription
    {
        public int MaxStealth { get; set; } = 999;
        public int MinStealth { get; set; } = 0;

        [OnRequestPlayCard]
        private void OnRequestPlayCard(object sender, RequestPlayCardEventArgs args)
        {
            if (args.CardId != Entity)
            {
                return;
            }

            if (Game.Player.Entity.GetComponent<Stealth>().Amount < MinStealth)
            {
                args.Blockers.Add($"Stealth Must be Greater than {MinStealth}!");
            }

            if (Game.Player.Entity.GetComponent<Stealth>().Amount > MaxStealth)
            {
                args.Blockers.Add($"Stealth Must be Less than {MaxStealth}!");
            }
        }

        public string Description
        {
            get
            {
                if (MaxStealth < 999 && MinStealth > 0)
                {
                    return $"Can't be played if Stealth is greater than {MinStealth} or less then {MaxStealth}";
                }

                if (MaxStealth < 99)
                {
                    return $"Can't be played if Stealth is greater than {MaxStealth}";
                }

                if (MinStealth > 0)
                {
                    return $"Can't be played if Stealth is lesz than {MinStealth}";
                }

                return $"Can't be played if Stealth is greater than {MinStealth} or less then {MaxStealth}";
            }
        }
    }

    public class AddCardToBattleDeck : SummerJam1Component, IEffect
    {
        public string Prefab { get; set; }
        public int Amount { get; set; } = 1;

        public bool DoEffect(IEntity target)
        {
            for (int i = 0; i < Amount; i++)
            {
                Context.CreateEntity(Game.Battle.BattleDeck.Entity, Prefab);
            }


            return true;
        }
    }


    public class DamageUnitEqualToMissingStealth : SummerJam1Component, IEffect
    {
        public bool DoEffect(IEntity target)
        {
            ITakesDamage unit = target.GetComponentInChildren<ITakesDamage>();

            if (unit == null)
            {
                return false;
            }


            unit.TryDealDamage(Game.Player.Entity.GetComponent<Stealth>().MaxStealth - Game.Player.Entity.GetComponent<Stealth>().Amount, Entity);
            return true;
        }
    }

    public class DealDamageForEachHealthLostThisTurn : SummerJam1Component, IEffect, IDescription
    {
        [JsonProperty] public int Multiplier { get; set; } = 1;

        private int PlayerStartHealth { get; set; }
        private int CurrentHealth { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            PlayerStartHealth = Game.Player.Entity.GetComponent<Health>().Amount;
            Game.Player.Entity.GetComponent<Health>().PropertyChanged += PlayerHealthChanged;
            UpdateCurrentHealth();
        }

        private void PlayerHealthChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateCurrentHealth();
        }

        private void UpdateCurrentHealth()
        {
            CurrentHealth = Game.Player.Entity.GetComponent<Health>().Amount;
        }

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            PlayerStartHealth = Game.Player.Entity.GetComponent<Health>().Amount;
        }


        public bool DoEffect(IEntity target)
        {
            ITakesDamage unit = target.GetComponentInChildren<ITakesDamage>();

            if (unit == null)
            {
                return false;
            }


            unit.TryDealDamage(Difference * Multiplier, Entity);
            return true;
        }

        private int Difference => Math.Max(0, PlayerStartHealth - CurrentHealth) + Entity.GetComponent<HealthCost>()?.Amount ?? 0;

        [PropertyChanged.DependsOn(nameof(CurrentHealth))]
        public string Description => $"Deal {Multiplier} damage for each health lost this turn. ({Difference * Multiplier})";
    }


    public class DealDamageForEachCardDrawnThisTurn : SummerJam1Component, IEffect, IDescription
    {
        [JsonProperty] public int Amount { get; set; } = 0;
        [JsonProperty] public int Multiplier { get; set; } = 1;

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            Amount = 0;
        }

        [OnCardDrawn]
        private void OnCardDrawn(object sender, CardDrawnEventArgs args)
        {
            if (args.IsHandDraw)
                return;


            Amount++;
        }

        public bool DoEffect(IEntity target)
        {
            ITakesDamage unit = target.GetComponentInChildren<ITakesDamage>();

            if (unit == null)
            {
                return false;
            }


            unit.TryDealDamage(Amount * Multiplier, Entity);
            return true;
        }

        public string Description => $"Deal {Multiplier} damage for each card drawn this turn. ({Amount * Multiplier})";
    }

    public class DamageUnitCard : SummerJam1Component, IEffect, IDescription
    {
        [JsonProperty] public int DamageAmount { get; private set; }
        [JsonProperty] public int Attacks { get; private set; } = 1;
        [JsonProperty] public bool Aoe { get; private set; }


        public bool DoEffect(IEntity target)
        {
            ITakesDamage unit = target.GetComponentInChildren<ITakesDamage>();

            if (unit == null)
            {
                return false;
            }

            unit.TryDealDamage(DamageAmount, Entity);
            if (Aoe)
            {
                foreach (IEntity entitiesInAdjacentSlot in Game.Battle.GetEntitiesInAdjacentSlots(target))
                {
                    var health = entitiesInAdjacentSlot.GetComponent<ITakesDamage>();
                    health.TryDealDamage(DamageAmount, Entity);
                }
            }
            return true;
        }

        public string Description
        {
            get
            {
                if (Aoe)
                {
                    return $"Deal {DamageAmount} damage to target and adjacent.";
                }
                return $"Deal {DamageAmount} damage.";
            }
        }
    }
}
