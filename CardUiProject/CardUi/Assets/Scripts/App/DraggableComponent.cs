using System.ComponentModel;
using CardsAndPiles;
using CardsAndPiles.Components;
using Stateless;
using UnityEngine;
using UnityEngine.EventSystems;

namespace App
{
    public class DraggableComponent : View<IDraggable>, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Vector3 TargetPosition { get; set; }

        private Vector3 Offset { get; set; }
        private bool Dragging { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            InputStateManager.Instance.StateMachine.OnTransitioned(OnInputStateChanged);
            SetEnabledState();
        }

        [PropertyListener(nameof(Draggable.CanDrag))]
        private void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            SetEnabledState();
        }


        private void OnInputStateChanged(StateMachine<InputState, InputAction>.Transition obj)
        {
            SetEnabledState();
        }

        private void SetEnabledState()
        {
            if (this == null || this.Model == null)
            {
                return; //hack
            }

            enabled = Model.CanDrag && InputStateManager.Instance.StateMachine.CanFire(InputAction.Drag);
        }

        // Update is called once per frame
        void Update()
        {
            if (Dragging)
            { //should lerp
                transform.position = TargetPosition;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 cursorWorld = GetWorldPoint(eventData);
            TargetPosition = new Vector3(Offset.x + cursorWorld.x, Offset.y + cursorWorld.y, transform.position.z);
        }

        private Vector3 GetWorldPoint(PointerEventData eventData)
        {
            return eventData.pressEventCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y,
                transform.position.z));
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Dragging = true;
            InputStateManager.Instance.StateMachine.Fire(InputAction.Drag);
            Vector3 cursorWorld = GetWorldPoint(eventData);

            Offset = transform.position - cursorWorld;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Dragging = false;
            InputStateManager.Instance.StateMachine.Fire(InputAction.EndDrag);

        }
    }
}
