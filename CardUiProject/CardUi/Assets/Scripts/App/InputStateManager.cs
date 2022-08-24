using System.Security.Permissions;
using App.Utility;
using Stateless;
using UnityEngine;

namespace App
{
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
                .Ignore(InputAction.PlayCard)
                .Permit(InputAction.Drag, InputState.Dragging)
                .Permit(InputAction.Hover, InputState.Hovering)
                .Permit(InputAction.ChooseDiscard, InputState.ChoosingDiscard)
                .Permit(InputAction.EndTurn, InputState.EnemyTurn)
                .Ignore(InputAction.EndHover)
                .Ignore(InputAction.EndDrag)
                ;
            StateMachine.Configure(InputState.Dragging)
                .Ignore(InputAction.PlayCard)
                .Permit(InputAction.EndDrag, InputState.Idle)
                .Permit(InputAction.ChooseDiscard, InputState.ChoosingDiscard)
                .PermitReentry(InputAction.Drag)
                ;
            StateMachine.Configure(InputState.Hovering)
                .Ignore(InputAction.PlayCard)
                .Permit(InputAction.ChooseDiscard, InputState.ChoosingDiscard)
                .Permit(InputAction.Drag, InputState.Dragging)
                .Permit(InputAction.EndHover, InputState.Idle)
                .PermitReentry(InputAction.Hover)
                ;
            StateMachine.Configure(InputState.ChoosingDiscard)
                .Permit(InputAction.EndChooseDiscard, InputState.Idle)
                .Ignore(InputAction.Hover)//Allow hover but don't change states.
                .Ignore(InputAction.EndHover)//Allow hover but don't change states.
            ;

            StateMachine.Configure(InputState.EnemyTurn)
                .Permit(InputAction.BeginTurn, InputState.Idle);
            
        }

        private void Update()
        {
            currentState = StateMachine.State.ToString();
        }
    }
}
