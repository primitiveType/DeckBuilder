using System;
using Content.Cards;
using DeckbuilderLibrary.Data.Events;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;


public class HandCardProxy : CardProxy, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMPro.TMP_Text NameText;
    [SerializeField] private TMPro.TMP_Text DescriptionText;
    [SerializeField] private TMPro.TMP_Text EnergyText;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        GameEntity.Context.Events.CardPlayed += EventsOnCardPlayed;
        UpdateCardText();
    }

    private void OnDestroy()
    {
        GameEntity.Context.Events.CardPlayed -= EventsOnCardPlayed;
    }

    private void EventsOnCardPlayed(object sender, CardPlayedEventArgs args)
    {
        UpdateCardText(); //any card being played could cause our text to need to update.
    }

    private void UpdateCardText()
    {
        EnergyText.text = $"{(GameEntity as EnergyCard)?.EnergyCost}";
        NameText.text = $"{GameEntity.Name}";
        DescriptionText.text = GameEntity.GetCardText();
    }

    public int HandPositionIndex
    {
        get => HandPositionIndex1;
        set => HandPositionIndex1 = value;
    }

    [SerializeField] private bool Hovered => MouseOver || Selected;

    private bool MouseOver;

    public bool Selected
    {
        get => m_Selected;

        set
        {
            m_Selected = value;
            HandleSelectedChanged();
        }
    }

    private Vector3 HandPosition { get; set; }

    private Vector3 TargetPosition { get; set; }

    [SerializeField] private Vector3 HoverOffset;

    [SerializeField] private Vector3 OffsetToPlayTargetlessCard;

    private bool m_Selected;

    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private Vector3 LineRendererStartOffset;
    private int HandPositionIndex1;

    public void ResetHandPosition(Vector3 handPosition)
    {
        HandPosition = handPosition;
        if (Hovered)
        {
            TargetPosition = HandPosition + HoverOffset;
        }
        else
        {
            TargetPosition = HandPosition;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MouseOver = true;
        TargetPosition = HandPosition + HoverOffset;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MouseOver = false;
        if (!Hovered)
        {
            TargetPosition = HandPosition;
        }
    }

    //void OnMouseOver()
    //{
    //    MouseOver = true;
    //    TargetPosition = HandPosition + HoverOffset;
    //}

    //void OnMouseExit()
    //{
    //    MouseOver = false;
    //    if (!Hovered)
    //    {
    //        TargetPosition = HandPosition;
    //    }
    //}

    //void OnMouseDown()
    //{
    //    if (!GameEntity.IsPlayable())
    //    {
    //        return;
    //    }
    //    if (!GameEntity.RequiresTarget)
    //    {
    //        GameEntity.PlayCard(null);
    //    }

    //    Selected = !Selected;
    //    TargetPosition = HandPosition + HoverOffset;
    //}

    private void HandleSelectedChanged()
    {
        if (Selected && GameEntity.RequiresTarget)
        {
            lineRenderer.enabled = true;
        }
        else if (Selected)
        {
        }
        else
        {
            lineRenderer.enabled = false;
            if (!Hovered)
            {
                TargetPosition = HandPosition;
            }
        }
    }

    void Start()
    {
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (transform.position != TargetPosition)
        {
            transform.position = Vector3.Lerp(transform.position, TargetPosition, .1f);
            if (Vector3.Distance(transform.position, TargetPosition) < .1f)
            {
                transform.position = TargetPosition;
            }
        }

        if (Selected)
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3 pointPosition = new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, .1f);
            lineRenderer.SetPosition(1, pointPosition);
            }

        lineRenderer.SetPosition(0, transform.position + LineRendererStartOffset);
    }
}
