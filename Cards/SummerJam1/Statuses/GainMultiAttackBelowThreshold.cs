using System.ComponentModel;
using Api;
using CardsAndPiles.Components;

namespace SummerJam1.Statuses
{
    public class GainMultiAttackBelowThreshold : SummerJam1Component, IAmount, ITooltip
    {
        public int Threshold { get; set; }

        public Health Health { get; set; }
        public int Amount { get; set; }
        public string Tooltip => $"Berserk - When below {Threshold} health, gain {Amount} multi-attack.";

        protected override void Initialize()
        {
            base.Initialize();
            Health = Entity.GetComponent<Health>();
            Health.PropertyChanged += HealthChanged;
            CheckForCondition();
        }

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
}