using System.Linq;
using Api;
using CardsAndPiles;

namespace SummerJam1.Rules
{
    public class DrawEncounterHandWhenEmpty : SummerJam1Component
    {
        private int CardDraw => 5;

        protected override void Initialize()
        {
            base.Initialize();
        }

        [OnCardPlayed]
        private void OnCardPlayed()
        {
            Logging.Log("Card discarded.");
            Game game = Context.Root.GetComponent<Game>();

            if (!game.Battle.EncounterHandPile.Entity.Children.Any())
            {
                Logging.Log("Hand empty.");

                for (int i = 0; i < CardDraw; i++)
                {
                    game.Battle.EncounterDrawPile.DrawCard(true);
                }
            }
        }
    }
}
