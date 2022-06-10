using CardsAndPiles;

namespace SummerJam1.Rules
{
    public class DiscardHandOnTurnEnd : SummerJam1Component
    {
        [OnTurnEnded]
        private void OnTurnEnded()
        {
            var game = Context.Root.GetComponent<SummerJam1Game>();
            foreach (Card componentsInChild in game.Hand.Entity.GetComponentsInChildren<Card>())
            {
                componentsInChild.Entity.TrySetParent(game.Discard);
            }
        }
    }

    public class DrawHandOnTurnBegin : SummerJam1Component
    {
        private int CardDraw => 5;

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            var game = Context.Root.GetComponent<SummerJam1Game>();

            for (int i = 0; i < CardDraw; i++)
            {
                game.Deck.DrawCard();
            }
        }
    }
}