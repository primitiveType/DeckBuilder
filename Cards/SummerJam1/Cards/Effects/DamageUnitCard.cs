using System;
using System.ComponentModel;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1.Cards.Effects
{
    public class DamageUnitEqualToStealth : SummerJam1Component, IEffect
    {
        public bool DoEffect(IEntity target)
        {
            ITakesDamage unit = target.GetComponentInChildren<ITakesDamage>();

            if (unit == null)
            {
                return false;
            }


            unit.TryDealDamage(Game.Player.CurrentStealth, Entity);
            return true;
        }
    }

    public class StealthRequirement : SummerJam1Component
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

            if (Game.Player.CurrentStealth < MinStealth)
            {
                args.Blockers.Add($"Stealth Must be Greater than {MinStealth}!");
            }

            if (Game.Player.CurrentStealth > MaxStealth)
            {
                args.Blockers.Add($"Stealth Must be Less than {MaxStealth}!");
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


            unit.TryDealDamage(Game.Player.MaxStealth - Game.Player.CurrentStealth, Entity);
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


            unit.TryDealDamage(Difference, Entity);
            return true;
        }

        private int Difference => Math.Max(0, PlayerStartHealth - CurrentHealth);
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

    public class DamageUnitCard : SummerJam1Component, IEffect
    {
        [JsonProperty] public int DamageAmount { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();
            if (Entity.GetComponent<DescriptionComponent>() == null)
            {
                Entity.GetOrAddComponent<DescriptionComponent>().Description =
                    $"Deal {DamageAmount} damage to target Unit.";
            }
        }


        public bool DoEffect(IEntity target)
        {
            if (!Entity.TrySetParent(target))
            {
                return false;
            }

            ITakesDamage unit = target.GetComponentInChildren<ITakesDamage>();

            if (unit == null)
            {
                return false;
            }


            unit.TryDealDamage(DamageAmount, Entity);
            return true;
        }
    }
}
