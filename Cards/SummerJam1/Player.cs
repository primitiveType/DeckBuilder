using System;
using CardsAndPiles;

namespace SummerJam1
{
    public class Player : SummerJam1Component
    {
        private int _currentStealth;
        public int CurrentEnergy { get; set; }
        public int MaxEnergy { get; private set; } = 3;

        public int CurrentStealth
        {
            get => _currentStealth;
            set => _currentStealth = Math.Min(Math.Max(value, 0), MaxStealth);
        }

        public int MaxStealth { get; private set; } = 10;

        public bool TryUseEnergy(int amount)
        {
            if (CurrentEnergy < amount)
            {
                return false;
            }

            CurrentEnergy -= amount;
            //invoke energy used.

            return true;
        }
        
        public bool TryUseStealth(int amount)
        {
            if (CurrentStealth < amount)
            {
                return false;
            }

            CurrentStealth -= amount;

            return true;
        }


        [OnTurnBegan]
        private void OnTurnBegan()
        {
            CurrentEnergy = MaxEnergy;
            CurrentStealth -= 1;
        }

        [OnBattleStarted]
        private void OnBattleStarted()
        {
            CurrentStealth = MaxStealth;
        }
        

        public override void Terminate()
        {
            base.Terminate();
            throw new Exception("Player terminated! NO!");
        }
    }
}
