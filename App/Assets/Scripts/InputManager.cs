using DeckbuilderLibrary.Data.GameEntities;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private HandPileProxy m_HandPileProxy;
    private HandPileProxy HandPileProxy => m_HandPileProxy;

    private SelectionDisplay m_SelectionDisplay;
    private SelectionDisplay SelectionDisplay => m_SelectionDisplay;

    private void Start()
    {
        m_HandPileProxy = GameObject.Find("HandPileProxy").GetComponent<HandPileProxy>();
        m_SelectionDisplay = GameObject.Find("SelectionDisplay").GetComponent<SelectionDisplay>();
    }

    private HandCardProxy SelectedCard { get; set; }

    private void Update()
    {
        if (Mouse.current.rightButton.isPressed)
        {
            if (SelectedCard != null)
            {
                SelectedCard.Selected = false;
                SelectedCard = null;
            }

            if (SelectionDisplay.IsDisplaying)
            {
                SelectionDisplay.ClearDisplay();
            }
        }
    }

    public void GameEntitySelected(IGameEntity gameEntity)
    {
        if (SelectedCard == null)
        {
            if (gameEntity is Card card)
            {
                IReadOnlyList<IGameEntity> validTargets = card.GetValidTargets();
                if (!card.IsPlayable() || (card.RequiresTarget && !validTargets.Any()))//if its not playable or has no valid targets
                {
                    //Negative visual feedback, i.e. card shake should play here.
                    return;
                }

                if (!card.RequiresTarget)
                {
                    card.PlayCard(null);
                }

                if (HandPileProxy.TryGetCardById(card.Id, out HandCardProxy handCardProxy))
                {
                    SelectedCard = handCardProxy;
                    IEnumerable<Card> cardTargets = validTargets.OfType<Card>().ToList();
                    if (cardTargets.Any())
                    {
                        SelectionDisplay.DisplaySelectableCards(cardTargets);
                    }
                    else if (validTargets.Any())
                    {
                        handCardProxy.Selected = true;
                    }
                }
            }
        }
        else
        {
            IReadOnlyList<IGameEntity> validTargets = SelectedCard.GameEntity.GetValidTargets();
            if (validTargets.Contains(gameEntity))
            {
                SelectedCard.GameEntity.PlayCard(gameEntity);
                SelectedCard.Selected = false;
                SelectedCard = null;
                if (SelectionDisplay.IsDisplaying)
                {
                    SelectionDisplay.ClearDisplay();
                }
            }
        }
    }
}