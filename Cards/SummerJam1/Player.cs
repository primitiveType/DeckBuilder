using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1
{
    public class Player : SummerJam1Component, ITooltip
    {
        public int CurrentEnergy { get; set; }
        public int MaxEnergy { get; } = 3;

        public string Tooltip => "Energy - Cards require energy to be played.";


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
        private void OnTurnBegan()
        {
            CurrentEnergy = MaxEnergy;
        }

        [OnEntityKilled]
        private void OnEntityKilled(object sender, EntityKilledEventArgs args)
        {
            if (args.Entity == Entity)
            {
                Events.OnBattleEnded(new BattleEndedEventArgs(false));
            }
        }
    }
}
