using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Cards;

namespace SummerJam1
{
    public class Player : SummerJam1Component, ITooltip
    {
        public int Movements { get; private set; }
        public int MovementsPerTurn { get; private set; }
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
            Movements = MovementsPerTurn;
        }

        [OnEntityKilled]
        private void OnEntityKilled(object sender, EntityKilledEventArgs args)
        {
            if (args.Entity == Entity)
            {
                Events.OnBattleEnded(new BattleEndedEventArgs(false));
            }
        }

        [OnRequestMoveUnit]
        private void OnRequestMoveUnit(object sender, RequestMoveUnitEventArgs args)
        {
            if (!args.UsesMovement)
            {
                return;
            }

            if (Movements == 0)
            {
                args.Blockers.Add(CardBlockers.NOT_ENOUGH_MOVEMENT);
            }
        }

        [OnUnitMoved]
        private void OnUnitMoved(object sender, UnitMovedEventArgs args)
        {
            if (args.UsesMovement)
            {
                Movements--;
            }
        }
    }
}
