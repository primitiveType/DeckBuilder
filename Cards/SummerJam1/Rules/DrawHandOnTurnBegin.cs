using CardsAndPiles;

namespace SummerJam1.Rules
{
    public class DrawHandOnTurnBegin : SummerJam1Component
    {
        private int CardDraw => 5;

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            var game = Context.Root.GetComponent<SummerJam1Game>();

            for (int i = 0; i < CardDraw; i++)
            {
                game.Battle.BattleDeck.DrawCard();
            }
        }
    }
}