using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Cards;

namespace SummerJam1.Rules
{
    public class DiscardHandOnDiscardPhase : SummerJam1Component
    {
        [OnDiscardPhaseBegan]
        private void OnDiscardPhaseBegan()
        {
            Game game = Context.Root.GetComponent<Game>();
            foreach (Card componentsInChild in game.Battle.Hand.Entity.GetComponentsInChildren<Card>())
            {
                if (componentsInChild.Entity.GetComponent<Retain>() == null)
                {
                    componentsInChild.Entity.TrySetParent(game.Battle.Discard);
                }
            }
        }
    }

    // public class DiscardDungeonHandOnTurnEnd : SummerJam1Component
    // {
    //     [OnDungeonPhaseEnded]
    //     private void OnDungeonPhaseEnded()
    //     {
    //         var game = Context.Root.GetComponent<Game>();
    //         foreach (Card componentsInChild in game.Battle.EncounterHandPile.Entity.GetComponentsInChildren<Card>())
    //         {
    //             if (componentsInChild.Entity.GetComponent<Retain>() == null)
    //             {
    //                 componentsInChild.Entity.TrySetParent(game.Battle.EncounterDiscardPile.Entity);
    //             }
    //         }
    //     }
    // }
}
