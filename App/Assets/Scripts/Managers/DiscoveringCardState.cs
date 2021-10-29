using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeckbuilderLibrary.Data;

public class DiscoveringCardState : State
{

    public override void Enter(StateData input)
    {
        base.Enter(input);
    }

    public override void Exit(StateData input)
    {
        base.Exit(input);
    }
    public override void EntityHovered(StateData input)
    {

    }

    public override void EntitySelected(StateData input)
    {
        if(InputManager.Instance.DiscoverPileProxy.TryGetCardById(input.Selected.Id, out DiscoverCardProxy cardProxy))
        {
            cardProxy.GameEntity.Context.TrySendToPile(cardProxy.GameEntity.Id, PileType.HandPile);


            InputManager.Instance.DiscoverPileProxy.ClearSelection();
            InputManager.Instance.TransitionToState(InputState.DefaultBattle);
        }
    }

    public override void EntityUnHovered(StateData input)
    {
     
    }
}
