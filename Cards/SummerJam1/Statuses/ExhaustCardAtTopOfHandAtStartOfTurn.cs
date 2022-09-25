using System.Linq;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Statuses
{
    public class ExhaustCardInHandAtStartOfTurn : EnabledWhenInEncounterSlot, IStatusEffect, ITooltip, IAmount
    {
        public int Amount { get; set; } = 1;
        public string Tooltip => $"At the start of every turn, consumes {Amount} card in the player's hand.";

        [OnTurnBegan]
        private void OnTurnBegan(object sender, TurnBeganEventArgs args)
        {
            for (int i = 0; i < Amount; i++)
            {
                Game.Battle.Hand.Entity.Children.FirstOrDefault()?.TrySetParent(Game.Battle.Exhaust);
            }
        }
    }
}
