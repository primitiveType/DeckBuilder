﻿using Api;
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

    public class FillSlotsOnTurnEnd : SummerJam1Component
    {
        [OnTurnBegan]
        private void OnDungeonPhaseEnded()
        {
            var game = Context.Root.GetComponent<Game>();
            foreach (Pile slot in game.Battle.GetEmptySlots())
            {
                game.Battle.EncounterDrawPile.DrawCardInto(slot.Entity);
            }
        }
    }
}
