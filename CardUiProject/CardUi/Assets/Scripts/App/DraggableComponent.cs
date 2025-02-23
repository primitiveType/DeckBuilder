using System.ComponentModel;
using System.Linq;
using CardsAndPiles;
using CardsAndPiles.Components;
using Stateless;
using SummerJam1.Cards.Effects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace App
{
    public class DraggableComponent : View<IDraggable>, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Vector3 TargetPosition { get; set; }
        private Vector3 CursorWorld { get; set; }

        private Vector3 Offset { get; set; }
        private bool Dragging { get; set; }
        private bool UnitTargeting { get; set; }

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

                if (UnitTargeting)
                {
                    TargetingManager.Instance.SetPosition(transform.position, CursorWorld);
                }
                else
                {
                    transform.position = TargetPosition;
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            CursorWorld = GetWorldPoint(eventData);
            TargetPosition = new Vector3(Offset.x + CursorWorld.x, Offset.y + CursorWorld.y, transform.position.z);
        }

        private Vector3 GetWorldPoint(PointerEventData eventData)
        {
            var eventDataPressEventCamera = eventData.pressEventCamera;
            return eventDataPressEventCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y,
                transform.position.z - eventDataPressEventCamera.transform.position.z));
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Dragging = true;
            InputStateManager.Instance.StateMachine.Fire(InputAction.Drag);
            CursorWorld = GetWorldPoint(eventData);

            Offset = transform.position - CursorWorld;

            if (Entity.GetComponents<IEffect>().Any(e => e.Targeting == TargetingType.Unit))
            {
                TargetingManager.Instance.Show(true);
                UnitTargeting = true;
            }
            else
            {
                UnitTargeting = false;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Dragging = false;
            InputStateManager.Instance.StateMachine.Fire(InputAction.EndDrag);
            TargetingManager.Instance.Show(false);
        }
    }
}
