using Api;
using CardsAndPiles.Components;
using SummerJam1.Statuses;

namespace SummerJam1.Units.Effects
{
    public class MoveLeftEachTurn : EnabledWhenAtTopOfEncounterSlot, IDescription
    {
        [OnMovementPhaseBegan]
        private void OnMovementPhaseBegan(object sender, MovementPhaseBeganEventArgs args)
        {
            if (!Game.Battle.TryMoveUnitLeft(Entity))
            {
                Logging.Log("Unable to move left.");
            }
        }

        public string Description => "Moving left.";
    }
}
