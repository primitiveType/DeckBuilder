using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Cards;

namespace SummerJam1.Rules
{
    public class DiscardHandOnTurnEnd : SummerJam1Component
    {
        [OnTurnEnded]
        private void OnTurnEnded()
        {
            var game = Context.Root.GetComponent<Game>();
            foreach (Card componentsInChild in game.Battle.Hand.Entity.GetComponentsInChildren<Card>())
            {
                if (componentsInChild.Entity.GetComponent<Retain>() == null)
                {
                    componentsInChild.Entity.TrySetParent(game.Battle.Discard);
                }
            }
        }
    }

   
}
