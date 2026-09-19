using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Cards
{
    public class CantPlayUntilEndOfTurn : SummerJam1Component, ITooltip
    {
        [OnRequestPlayCard]
        private void OnRequestPlayCard(object sender, RequestPlayCardEventArgs args)
        {
            if (args.CardId == Entity)
            {
                args.Blockers.Add("That card is locked until next turn!");
            }
        }
        
        [OnTurnEnded]
        private void OnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            Entity.RemoveComponent(this);
        }

        public string Tooltip { get; } = "This card can not be played until next turn.";
    }
}