using System;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1
{
    public class Stealth : SummerJam1Component, ITooltip, IAmount
    {
        private int _amount;

        public int Amount
        {
            get => _amount;
            set => _amount = Math.Max(0, value);
        }

        public int MaxStealth { get; set; } = 5;

        public bool TryUseStealth(int amount)
        {
            if (Amount < amount)
            {
                Amount = 0;
                return false;
            }

            Amount -= amount;

            return true;
        }


        [OnAttackPhaseEnded]
        private void OnAttackPhaseEnded()
        {
            Amount -= 1;
        }

        [OnBattleStarted]
        private void OnBattleStarted()
        {
            Amount = MaxStealth;
        }

        public string Tooltip => "Stealth - While above zero, enemies will not attack.";
    }
}
