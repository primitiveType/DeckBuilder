using System;
using System.ComponentModel;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using PropertyChanged;

namespace SummerJam1.Cards.Effects
{
    public class DealDamageForEachHealthLostThisTurn : SummerJam1Component, IEffect, IDescription
    {
        [JsonProperty] public int Multiplier { get; set; } = 1;

        private int PlayerStartHealth { get; set; }
        private int CurrentHealth { get; set; }

        private int Difference => Math.Max(0, PlayerStartHealth - CurrentHealth) + Entity.GetComponent<HealthCost>()?.Amount ?? 0;

        [DependsOn(nameof(CurrentHealth))]
        public string Description => $"Deal {Multiplier} damage for each health lost this turn. ({Difference * Multiplier})";


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
    }
}