using CardsAndPiles;

namespace SummerJam1.Rules
{
    public class DrawHandOnTurnBegin : SummerJam1Component
    {
        private int CardDraw => 5;

        [OnDrawPhaseBegan]
        private void OnDrawPhaseBegan()
        {
            var game = Context.Root.GetComponent<Game>();

            for (int i = 0; i < CardDraw; i++)
            {
                game.Battle.BattleDeck.DrawCard(true);
            }
        }
    }
    public class DrawEncounterHandOnTurnBegin : SummerJam1Component
    {
        private int CardDraw => 5;

        [OnDungeonPhaseStarted]
        private void OnDungeonPhaseStarted()
        {
            var game = Context.Root.GetComponent<Game>();

            for (int i = 0; i < CardDraw; i++)
            {
                game.Battle.EncounterDrawPile.DrawCard(true);
            }
        }
    }
}
