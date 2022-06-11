using System.Collections;
using Stateless;
using UnityEngine;
using UnityEngine.EventSystems;

namespace App
{
    public class CardInHand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private bool IsHovered { get; set; }
        public IPileItemView PileItemView { get; private set; }

        public bool DisplayWholeCard => IsHovered && enabled && WarmedUp;
        private bool WarmedUp { get; set; }

        private void Awake()
        {
            PileItemView = GetComponentInParent<IPileItemView>();
            if (PileItemView == null)
            {
                Debug.LogError("Pile item not found when adding card to hand!");
            }

            StartCoroutine(WarmUp());
        }

        private IEnumerator WarmUp()
        {
            yield return new WaitForSeconds(.5f);
            WarmedUp = true;
        }


        private void Start()
        {
            InputStateManager.Instance.StateMachine.OnTransitioned(OnInputStateChanged);
            SetEnabledStateFromMachine();
        }


        private void OnInputStateChanged(StateMachine<InputState, InputAction>.Transition obj)
        {
            if (this == null)
            {
                return;//hack for now. Can't unsubscribe from state machine.
            }

            SetEnabledStateFromMachine();
        }

        private void SetEnabledStateFromMachine()
        {
            enabled = InputStateManager.Instance.StateMachine.CanFire(InputAction.Hover);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsHovered = true;
            InputStateManager.Instance.StateMachine.Fire(InputAction.Hover);
            transform.localScale = Vector3.one * 1.2f;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsHovered = false;
            InputStateManager.Instance.StateMachine.Fire(InputAction.EndHover);

            ResetScale();
        }

        private void OnDisable()
        {
            ResetScale();
            IsHovered = false; //this feels better but its a little odd.
        }

        private void ResetScale()
        {
            transform.localScale = Vector3.one;
        }
    }
}