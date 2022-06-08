using System;
using System.ComponentModel;
using CardsAndPiles;
using Common;
using Stateless;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableComponent : View<IDraggable>, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private Vector3 targetPosition { get; set; }

    private Vector3 offset { get; set; }
    private bool dragging { get; set; }

    protected override void Start()
    {
        base.Start();
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
        if (dragging)
        { //should lerp
            transform.position = targetPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 cursorWorld = GetWorldPoint(eventData);
        targetPosition = new Vector3(offset.x + cursorWorld.x, offset.y + cursorWorld.y, transform.position.z);
    }

    private Vector3 GetWorldPoint(PointerEventData eventData)
    {
        return eventData.pressEventCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y,
            transform.position.z));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = true;
        InputStateManager.Instance.StateMachine.Fire(InputAction.Drag);
        Vector3 cursorWorld = GetWorldPoint(eventData);

        offset = transform.position - cursorWorld;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
        InputStateManager.Instance.StateMachine.Fire(InputAction.EndDrag);

    }
}