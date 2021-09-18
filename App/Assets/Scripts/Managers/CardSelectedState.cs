using System;
using System.Collections.Generic;
using System.Linq;
using DeckbuilderLibrary.Data.GameEntities;

public class CardSelectedState : State
{
    private HandCardProxy SelectedCard { get; set; }

    public override void Enter(StateData input)
    {
        base.Enter(input);
        //TODO: keep nodes highlighted! easy
        InputManager.Instance.HandPileProxy.TryGetCardById(input.Selected.Id, out HandCardProxy selectedCard);
        SelectedCard = selectedCard;
        SelectedCard.Selected = true;
        if (!SelectedCard.GameEntity.RequiresTarget)
        {
            SelectedCard.GameEntity.PlayCard(null);
            InputManager.Instance.TransitionToState(InputManager.InputState.DefaultBattle);
        }
        else
        {
            List<Card> targetCards = SelectedCard.GameEntity.GetValidTargets().OfType<Card>().ToList();
            if (targetCards.Any())
            {
                InputManager.Instance.SelectionDisplay.DisplaySelectableCards(targetCards);
            }
            else
            {
                List<ActorNode> targetNodes = SelectedCard.GameEntity.GetValidTargets().OfType<ActorNode>().ToList();
                foreach (var node in targetNodes)
                {
                    HighlightHandles.Add(HighlightNode<NodeHighlightPathEffectComponent>(node));
                }
            }
        }
    }

    public override void Exit(StateData input)
    {
        base.Exit(input);
        SelectedCard = null;
        ClearTempHoverStates();
    }

    private void ClearTempHoverStates()
    {
        foreach (IDisposable handle in TempHoverHandles)
        {
            handle.Dispose();
        }

        TempHoverHandles.Clear();
    }

    public override void EntitySelected(StateData input)
    {
        IReadOnlyList<IGameEntity> validTargets = SelectedCard.GameEntity.GetValidTargets();
        if (validTargets.Contains(input.Selected))
        {
            SelectedCard.GameEntity.PlayCard(input.Selected);
            SelectedCard.Selected = false;
            SelectedCard = null;
            if (InputManager.Instance.SelectionDisplay.IsDisplaying)
            {
                InputManager.Instance.SelectionDisplay.ClearDisplay();
            }

            InputManager.Instance.TransitionToState(InputManager.InputState.DefaultBattle);
        }
    }

    private List<IDisposable> TempHoverHandles = new List<IDisposable>();

    public override void EntityHovered(StateData input)
    {
        IReadOnlyList<IGameEntity> validTargets = SelectedCard.GameEntity.GetValidTargets();
        if (input.Selected is ActorNode target && validTargets.Contains(target))
        {
            foreach (var affected in SelectedCard.GameEntity.GetAffectedEntities(target))
            {
                if (affected is ActorNode node)
                {
                    TempHoverHandles.Add(HighlightNode<NodeHighlightThreatEffectComponent>(node));
                }
            }
        }
    }

    public override void EntityUnHovered(StateData input)
    {
        ClearTempHoverStates();
    }
}