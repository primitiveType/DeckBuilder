using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1
{
    public class Player : SummerJam1Component, ITooltip
    {
        public int CurrentEnergy { get; set; }
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
        private void OnTurnBegan()
        {
            CurrentEnergy = MaxEnergy;
        }

        public string Tooltip => "Energy - Cards require energy to be played.";
    }

    public class Money : SummerJam1Component, IAmount
    {
        public int Amount { get; set; }
    }

    public class DeathAddsMoneyToPlayer : SummerJam1Component
    {
        [OnEntityKilled]
        private void OnEntityKilled(object sender, EntityKilledEventArgs args)
        {
            if (args.Entity == Entity)
            {
                var amount = Entity.GetComponent<Money>().Amount;
                Game.Player.Entity.GetOrAddComponent<Money>().Amount += amount;
            }
        }
    }
}
