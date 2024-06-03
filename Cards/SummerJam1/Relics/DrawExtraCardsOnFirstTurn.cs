using Api.Extensions;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Relics
{
    public class DrawExtraCardsOnFirstTurn : SummerJam1Component, IDescription
    {
        public int Amount { get; set; }

        private bool IsFirstTurn { get; set; }

        public string Description => $"Draw {Amount} extra card{Amount.ToPluralitySuffix()} on your first turn each combat.";

        [OnBattleStarted]
        private void OnBattleStarted()
        {
            IsFirstTurn = true;
        }

        [OnTurnBegan]
        private void OnTurnBegan(object sender, TurnBeganEventArgs args)
        {
            if (!IsFirstTurn)
            {
                return;
            }

            IsFirstTurn = false;

            for (int i = 0; i < Amount; i++)
            {
                Game.Battle.BattleDeck.DrawCard();
            }
        }
    }
}