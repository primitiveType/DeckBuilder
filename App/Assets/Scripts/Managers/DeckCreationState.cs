using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.GameEntities;

public class DeckCreationState : State
{
    private IContext Context => GameContextManager.Instance.Context;

    public override void EntityHovered(StateData input)
    {
      
    }

    public override void EntitySelected(StateData input)
    {
        Type cardType = input.Selected.GetType();

        Context.PlayerDeck.Add((Card)Context.CreateEntity(cardType));


    }

    public override void EntityUnHovered(StateData input)
    {
        
    }
}
