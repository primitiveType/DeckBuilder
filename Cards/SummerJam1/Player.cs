using System;
using CardsAndPiles;

namespace SummerJam1
{
    public class Player : SummerJam1Component
    {
        public int CurrentEnergy { get; private set; }
        public int MaxEnergy { get; private set; } = 3;

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


        [OnTurnBegan]
        [OnBattleStarted]
        private void OnTurnBegan()
        {
            CurrentEnergy = MaxEnergy;
        }

        public override void Terminate()
        {
            base.Terminate();
            throw new Exception("Player terminated! NO!");
        }
    }
}