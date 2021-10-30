using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.GameEntities;

public class DiscoveringCardState : State
{
    public override void Enter(StateData input)
    {
        base.Enter(input);
    }

    public override void Exit(StateData input)
    {
        base.Exit(input);
        foreach (Card discoverPileCard in GameContext.CurrentContext.GetCurrentBattle().Deck.DiscoverPile.Cards)
        {
            discoverPileCard.Destroy(); //not really sure if this is correct. could be problematic in the future.
        }

        GameContext.CurrentContext.GetCurrentBattle().Deck.DiscoverPile.Cards.Clear();
    }

    public override void EntityHovered(StateData input)
    {
    }

    public override void EntitySelected(StateData input)
    {
        if (input.Selected is Card card &&
            GameContext.CurrentContext.GetCurrentBattle().Deck.DiscoverPile.Cards.Contains(card))
        {
            GameContext.CurrentContext.TrySendToPile(input.Selected.Id, PileType.DrawPile);//TODO: figure out how to send them to correct pile.
            //Seems like we need more flexibility with input states knowing more.
            //We might also need a stack of states internal to the library that require resolution.
            //I can see things breaking if we have an event that interrupts an input state incorrectly.
            InputManager.Instance.TransitionToState(InputState.DefaultBattle);
        }
    }

    public override void EntityUnHovered(StateData input)
    {
    }
}