using System;
using System.Security.Permissions;
using Stateless;
using UnityEngine;
using State = Stateless.Graph.State;

public class InputStateManager : MonoBehaviourSingleton<InputStateManager>
{
    public StateMachine<InputState, InputAction> StateMachine { get; private set; }

    [SerializeField] private string currentState;

    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        StateMachine = new StateMachine<InputState, InputAction>(InputState.Idle);
        StateMachine.Configure(InputState.Idle)
            .Permit(InputAction.Drag, InputState.Dragging)
            .Permit(InputAction.Hover, InputState.Hovering)
            .Ignore(InputAction.EndHover)
            .Ignore(InputAction.EndDrag);
        StateMachine.Configure(InputState.Dragging)
            .Permit(InputAction.EndDrag, InputState.Idle)
            .PermitReentry(InputAction.Drag);
        StateMachine.Configure(InputState.Hovering)
            .Permit(InputAction.Drag, InputState.Dragging)
            .Permit(InputAction.EndHover, InputState.Idle)
            .PermitReentry(InputAction.Hover);
    }

    private void Update()
    {
        currentState = StateMachine.State.ToString();
    }
}