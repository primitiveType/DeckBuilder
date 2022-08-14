using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Statuses
{
    public class Regen : EnabledWhenInEncounterSlot, IAmount, IStatusEffect
    {
        public int Amount { get; set; }

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            Entity.GetComponent<Health>().TryHeal(Amount, Entity);
            Amount--;
            if (Amount <= 0)
            {
                Entity.RemoveComponent(this);
            }
        }
    }

    public class GainArmorEveryTurn : EnabledWhenInEncounterSlot, IStatusEffect, ITooltip, IAmount
    {
        [PropertyChanged.DependsOn(nameof(Amount))]
        public string Tooltip => $"Gains {Amount} Armor every turn.";

        public int Amount { get; set; }

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            Entity.GetOrAddComponent<Armor>().Amount += Amount;
        }
    }

    public class PlayerHealingHealsMe : EnabledWhenInEncounterSlot, IStatusEffect, ITooltip
    {
        public string Tooltip => $"Whenever the player heals, this unit heals for the same amount.";

        [OnHealDealt]
        private void OnHealDealt(object sender, HealDealtEventArgs args)
        {
            if (args.EntityId == Game.Player.Entity)
            {
                Entity.GetComponent<Health>().TryHeal(args.Amount, Entity);
            }
        }
    }

    public class OnlyPlayerSelfDamageDamagesMe : SummerJam1Component, IStatusEffect, ITooltip
    {
        public string Tooltip => $"Unnatural - this creature only takes damage when you damage yourself.";

        [OnDamageDealt]
        private void OnDamageDealt(object sender, DamageDealtEventArgs args)
        {
            if (args.EntityId == Game.Player.Entity && args.SourceEntityId != Entity)
            {
                Entity.GetComponent<Health>().TryDealDamage(args.Amount, Entity);
            }
        }

        [OnRequestDamageMultipliers]
        private void OnRequestDamageMultipliers(object sender, RequestDamageMultipliersEventArgs args)
        {
            if (args.Target == Entity && args.Source != Entity)
            {
                args.Multiplier.Add(0);
            }
        }
    }

    public class GainStrengthWhenPlayerHeals : EnabledWhenInEncounterSlot, IAmount, ITooltip
    {
        public int Amount { get; set; }
        public string Tooltip => $"Tough - Whenever you heal, gains {Amount} Strength";

        [OnHealDealt]
        private void OnHealDealt(object sender, HealDealtEventArgs args)
        {
            if (args.EntityId == Game.Player.Entity)
            {
                Entity.GetOrAddComponent<Strength>().Amount += Amount;
            }
        }
    }

    public class GainMultiAttackBelowThreshold : SummerJam1Component, IAmount, ITooltip
    {
        public int Threshold { get; set; }
        public int Amount { get; set; }
        public string Tooltip => $"Berserk - When below {Threshold} health, gain {Amount} multi-attack.";

        protected override void Initialize()
        {
            base.Initialize();
            Health = Entity.GetComponent<Health>();
            Health.PropertyChanged += HealthChanged;
            CheckForCondition();
        }

        public Health Health { get; set; }

        private void HealthChanged(object sender, PropertyChangedEventArgs e)
        {
            CheckForCondition();
        }

        private void CheckForCondition()
        {
            if (Health.Amount < Threshold)
            {
                Entity.GetOrAddComponent<MultiAttack>().Amount += Amount;
                Entity.RemoveComponent(this);
            }
        }

        public override void Terminate()
        {
            base.Terminate();
            Health.PropertyChanged -= HealthChanged;
        }
    }

    public class ExhaustCardInHandAtStartOfTurn : EnabledWhenInEncounterSlot, IStatusEffect, ITooltip, IAmount
    {
        public string Tooltip => $"At the start of every turn, consumes {Amount} card in the player's hand.";

        [OnTurnBegan]
        private void OnTurnBegan(object sender, TurnBeganEventArgs args)
        {
            for (int i = 0; i < Amount; i++)
            {
                Game.Battle.Hand.Entity.Children.FirstOrDefault()?.TrySetParent(Game.Battle.Exhaust);
            }
        }

        public int Amount { get; set; } = 1;
    }

    public abstract class EnabledWhenInEncounterSlot : SummerJam1Component
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.PropertyChanged += EntityOnPropertyChanged;
            UpdateEnabledState();
        }

        private void EntityOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Entity.Parent))
            {
                UpdateEnabledState();
            }
        }

        private void UpdateEnabledState()
        {
            Enabled = Entity.GetComponentInParent<EncounterSlotPile>() != null;
        }

        public override void Terminate()
        {
            base.Terminate();
            Entity.PropertyChanged -= EntityOnPropertyChanged;
        }
    }
    
    public class DiscardCardInHandAtStartOfTurn : EnabledWhenInEncounterSlot, IStatusEffect, ITooltip, IAmount
    {
        public string Tooltip => $"Scary - At the start of every turn, discards {Amount} card in the player's hand.";

        [OnTurnBegan]
        private void OnTurnBegan(object sender, TurnBeganEventArgs args)
        {
            if (!Enabled)
            {
                return;
            }
            for (int i = 0; i < Amount; i++)
            {
                Game.Battle.Hand.Entity.Children.FirstOrDefault()?.TrySetParent(Game.Battle.Discard);
            }
        }

        public int Amount { get; set; } = 1;
    }


    public class ReducePlayerStealthAtStartOfCombat : EnabledWhenInEncounterSlot, IStatusEffect, ITooltip, IAmount
    {
        public int Amount { get; set; }

        [OnBattleStarted]
        public void OnBattleStarted(object sender, BattleStartedEventArgs args)
        {
            Game.Player.Entity.GetComponent<Stealth>().TryUseStealth(Amount);
        }

        public string Tooltip => $"At start of battle, reduces the player's stealth by {Amount}";
    }
}
