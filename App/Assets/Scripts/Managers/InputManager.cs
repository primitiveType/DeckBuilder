using DeckbuilderLibrary.Data.GameEntities;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.Assertions.Must;
using UnityEngine.InputSystem;


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