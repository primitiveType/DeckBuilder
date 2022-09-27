using System.Linq;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Statuses
{
    public class DiscardCardInHandAtStartOfTurn : EnabledWhenInEncounterSlot, IStatusEffect, ITooltip, IAmount
    {
        public int Amount { get; set; } = 1;
        public string Tooltip => $"Scary - At the start of every turn, discards {Amount} card in the player's hand.";

        [OnTurnBegan]
        private void OnTurnBegan(object sender, TurnBeganEventArgs args)
        {
            if (!Enabled)
            {
                return;
            }

            for (int i = 0; i < Amount; i++)
            {
                Game.Battle.Hand.Entity.Children.FirstOrDefault()?.TrySetParent(Game.Battle.Discard);
            }
        }
    }
}