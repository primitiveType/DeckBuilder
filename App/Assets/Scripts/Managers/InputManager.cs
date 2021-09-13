using System;
using DeckbuilderLibrary.Data.GameEntities;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading.Tasks;
using ca.axoninteractive.Geometry.Hex;
using DeckbuilderLibrary.Data.DataStructures;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using Scenes.Scripts.StateMachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;


public class InputManager : MonoBehaviourSingleton<InputManager>
{
    private HandPileProxy m_HandPileProxy;
    public HandPileProxy HandPileProxy => m_HandPileProxy;

    private SelectionDisplay m_SelectionDisplay;
    public SelectionDisplay SelectionDisplay => m_SelectionDisplay;

    private State CurrentState { get; set; } = new DefaultBattleState();

    public enum InputState
    {
        DefaultBattle,
        CardSelected
    }


    private Dictionary<InputState, State> States = new Dictionary<InputState, State>
    {
        { InputState.DefaultBattle, new DefaultBattleState() },
        { InputState.CardSelected, new CardSelectedState() }
    };

    public void TransitionToState(InputState state)
    {
        CurrentState?.Exit(new StateData(SelectedEntity));
        CurrentState = States[state];
        CurrentState.Enter(new StateData(SelectedEntity));
    }

    private void Start()
    {
        TransitionToState(InputState.DefaultBattle);
        m_HandPileProxy = GameObject.Find("HandPileProxy").GetComponent<HandPileProxy>();
        m_SelectionDisplay = GameObject.Find("SelectionDisplay").GetComponent<SelectionDisplay>();
    }


    private void Update()
    {
        if (Mouse.current.rightButton.isPressed)
        {
            GameEntitySelected(null);
        }
    }

    public void GameEntitySelected(IGameEntity gameEntity)
    {
        SelectedEntity = gameEntity;
        CurrentState.EntitySelected(new StateData(gameEntity));
    }

    public IGameEntity SelectedEntity { get; set; }

    public void GameEntityUnHovered(IGameEntity gameEntity)
    {
        CurrentState.EntityUnHovered(new StateData(gameEntity));
    }

    public void GameEntityHovered(IGameEntity gameEntity)
    {
        CurrentState.EntityHovered(new StateData(gameEntity));
    }
}

public struct StateData
{
    public readonly IGameEntity Selected;

    public StateData(IGameEntity selected)
    {
        Selected = selected;
    }
}

public abstract class State : IState<StateData>
{
    public abstract void Enter(StateData input);
    public abstract void Exit(StateData input);
    public abstract void EntitySelected(StateData input);
    public abstract void EntityHovered(StateData input);
    public abstract void EntityUnHovered(StateData input);
}

public class DefaultBattleState : State
{
    private List<IDisposable> HoverHandles { get; } = new List<IDisposable>();

    public override void EntityUnHovered(StateData input)
    {
        ClearHoverStates();
    }

    private void ClearHoverStates()
    {
        foreach (IDisposable handle in HoverHandles)
        {
            handle.Dispose();
        }

        HoverHandles.Clear();
    }

    public override void EntitySelected(StateData input)
    {
        if (input.Selected is Card card)
        {
            IReadOnlyList<IGameEntity> validTargets = card.GetValidTargets();
            if (!card.IsPlayable() ||
                (card.RequiresTarget && !validTargets.Any())) //if its not playable or has no valid targets
            {
                //Negative visual feedback, i.e. card shake should play here.
                return;
            }


            InputManager.Instance.TransitionToState(InputManager.InputState.CardSelected);
        }
        else if (input.Selected is ActorNode node)
        {
            PlayerActor player = node.Context.GetCurrentBattle().Player;
            ActorNode playerNode = node.Context.GetCurrentBattle().Graph.GetNodeOfActor(player);
            ActorNodePath path =
                new ActorNodePath(playerNode, node, false, node.GetActor() == null);

            MoveToPath(player, path);
        }


        ClearHoverStates();
    }

    private async Task MoveToPath(IActor actor, Path<ActorNode> path)
    {
        foreach (ActorNode node in path)
        {
            if (node.GetActor() == actor)
            {
                continue;
            }

            node.Context.GetCurrentBattle().Graph.MoveIntoSpace(actor, node);
            await Task.Delay(TimeSpan.FromSeconds(.25f));
        }
    }


    public override void Enter(StateData input)
    {
        //TODO
        Battle = Object.FindObjectOfType<BattleProxy>();
    }

    private BattleProxy Battle { get; set; }

    public override void Exit(StateData input)
    {
    }


    public override void EntityHovered(StateData input)
    {
        if (input.Selected is Card card)
        {
            IReadOnlyList<IGameEntity> targets = card.GetValidTargets();
            if (targets != null)
            {
                foreach (IGameEntity target in targets)
                {
                    if (target is ActorNode node)
                    {
                        HighlightNode<NodeHighlightThreatEffectComponent>(node);
                    }
                }
            }
        }
        else if (input.Selected is ActorNode node)
        {
            PlayerActor player = node.Context.GetCurrentBattle().Player;
            ActorNode playerNode = node.Context.GetCurrentBattle().Graph.GetNodeOfActor(player);
            ActorNodePath path =
                new ActorNodePath(playerNode, node, false, node.GetActor() == null);
            foreach (ActorNode pathnode in path)
            {
                HighlightNode<NodeHighlightPathEffectComponent>(pathnode);
            }
        }
        else if (input.Selected is Enemy enemy)
        {
            List<CubicHexCoord> affected = enemy.Intent.Target.GetAffectedCoordinates(enemy.Coordinate,
                enemy.Intent.TargetNode.Coordinate);
            IReadOnlyDictionary<CubicHexCoord, ActorNode> nodes = Battle.GameEntity.Graph.GetNodes();
            foreach (CubicHexCoord affectedNode in affected)
            {
                if (nodes.TryGetValue(affectedNode, out ActorNode entity))
                {
                    HighlightNode<NodeHighlightThreatEffectComponent>(entity);
                }
            }
            ActorNodeRange range = new ActorNodeRange(enemy.Context.GetCurrentBattle().Graph.GetNodeOfActor(enemy), enemy.MoveSpeed);
            foreach (var rangeNode in range.Nodes)
            {
                HighlightNode<NodeHighlightPathEffectComponent>(rangeNode);
            }
        }
    }

    private void HighlightNode<T>(ActorNode pathnode) where T : EffectComponent
    {
        HoverHandles.Add(Battle.GetNodeProxyByEntity(pathnode)
            .GetComponentInChildren<T>(true)
            .GetEffectHandle());
    }
}


public class CardSelectedState : State
{
    private HandCardProxy SelectedCard { get; set; }

    public override void Enter(StateData input)
    {
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
            IEnumerable<Card> targetCards = SelectedCard.GameEntity.GetValidTargets().OfType<Card>();
            if (targetCards.Any())
            {
                InputManager.Instance.SelectionDisplay.DisplaySelectableCards(targetCards);
            }
        }
    }

    public override void Exit(StateData input)
    {
        SelectedCard = null;
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

    public override void EntityHovered(StateData input)
    {
    }

    public override void EntityUnHovered(StateData input)
    {
    }
}