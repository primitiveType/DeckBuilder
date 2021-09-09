using System.Collections.Generic;
using System.Linq;
using DeckbuilderLibrary.Data.GameEntities;
using UnityEngine;

public class SelectableComponent : GameEntityComponent
{
    private void OnMouseDown()
    {
        //TODO Don't use GameObject.Find 
        var handPileProxy = GameObject.Find("HandPileProxy").GetComponent<HandPileProxy>();
        IReadOnlyList<HandCardProxy> selectedCards = handPileProxy.GetSelectedCards();
        foreach (HandCardProxy selectedCard in selectedCards)
        {
            Card card = selectedCard.GameEntity;
            IReadOnlyList<IGameEntity> validTargets = card.GetValidTargets();
            if (validTargets.Contains(GameEntity))
            {
                card.PlayCard(GameEntity);
            }

            selectedCard.Selected = false;
        }
    }
}