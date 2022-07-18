using System;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1
{
    public class Stealth : SummerJam1Component, ITooltip
    {
        private int _currentStealth;

        public int CurrentStealth
        {
            get => _currentStealth;
            set => _currentStealth = Math.Min(Math.Max(value, 0), MaxStealth);
        }

        public int MaxStealth { get; set; } = 5;

        public bool TryUseStealth(int amount)
        {
            if (CurrentStealth < amount)
            {
                return false;
            }

            CurrentStealth -= amount;

            return true;
        }


        [OnTurnEnded]
        private void OnTurnEnded()
        {
            CurrentStealth -= 1;
        }

        [OnBattleStarted]
        private void OnBattleStarted()
        {
            CurrentStealth = MaxStealth;
        }

        public string Tooltip => "Stealth - While above zero, enemies will not attack.";
    }
}
