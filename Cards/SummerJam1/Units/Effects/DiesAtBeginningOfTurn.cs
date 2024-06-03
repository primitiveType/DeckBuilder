using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Units.Effects
{
    public class DiesAtBeginningOfTurn : SummerJam1Component, IDescription
    {
        public string Description => "Expires at the beginning of the next turn.";

        [OnAttackPhaseEnded]
        private void OnTurnBegan(object sender, AttackPhaseEndedEventArgs args)
        {
            Entity.Destroy();//might be more interesting if it actually triggers a "kill" event.
        }
    }
}
