using CardsAndPiles;

namespace SummerJam1.Rules
{
    public class DiscardHandOnTurnEnd : SummerJam1Component
    {
        [OnTurnEnded]
        private void OnTurnEnded()
        {
            var game = Context.Root.GetComponent<SummerJam1Game>();
            foreach (Card componentsInChild in game.Hand.GetComponentsInChildren<Card>())
            {
                componentsInChild.Entity.TrySetParent(game.Discard);
            }
        }
    }
}