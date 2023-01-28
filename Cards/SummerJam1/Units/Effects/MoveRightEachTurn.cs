using Api;
using CardsAndPiles.Components;
using SummerJam1.Statuses;

namespace SummerJam1.Units.Effects
{
    public class MoveRightEachTurn : EnabledWhenAtTopOfEncounterSlot, IDescription
    {
        [OnMovementPhaseBegan]
        private void OnMovementPhaseBegan(object sender, MovementPhaseBeganEventArgs args)
        {
            if (!Game.Battle.TryMoveUnitRight(Entity))
            {
                Logging.Log("Unable to move to the right.");
            }
        }

        public string Description => "Moving right.";
    }
}
