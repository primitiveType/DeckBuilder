namespace SummerJam1.Rules
{
    public class DrawEncounterHandOnTurnBegin : SummerJam1Component
    {
        private int CardDraw => 5;

        [OnDungeonPhaseStarted]
        private void OnDungeonPhaseStarted()
        {
            Game game = Context.Root.GetComponent<Game>();

            for (int i = 0; i < CardDraw; i++)
            {
                game.Battle.EncounterDrawPile.DrawCard(true);
            }
        }
    }
}
