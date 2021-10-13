using DeckbuilderLibrary.Data.GameEntities;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum InputState
{
    DefaultBattle,
    CardSelected,
    DiscoveringCard,
    DeckCreation
}

public  class InputManager : MonoBehaviourSingleton<InputManager> {

    [SerializeField]
    private HandPileProxy m_HandPileProxy;
    public HandPileProxy HandPileProxy => m_HandPileProxy;

    [SerializeField]
    private DiscoverPileProxy m_DiscoverPileProxy;
    public DiscoverPileProxy DiscoverPileProxy => m_DiscoverPileProxy;

    [SerializeField]
    private SelectionDisplay m_SelectionDisplay;
    public SelectionDisplay SelectionDisplay => m_SelectionDisplay;

    private State CurrentState { get; set; } = new DefaultBattleState();

    private Dictionary<InputState, State> States = new Dictionary<InputState, State>
    {
        { InputState.DefaultBattle, new DefaultBattleState() },
        { InputState.CardSelected, new CardSelectedState() },
        { InputState.DiscoveringCard, new DiscoveringCardState() },
        {InputState.DeckCreation, new DeckCreationState() }
    };

    public void TransitionToState(InputState state)
    {
        
        CurrentState?.Exit(new StateData(SelectedEntity));
        CurrentState = States[state];
        CurrentState.Enter(new StateData(SelectedEntity));
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
        Debug.Log("Selected entity in state: " + CurrentState);
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