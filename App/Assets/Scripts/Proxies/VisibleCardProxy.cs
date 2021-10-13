using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Content.Cards;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.Events;

public class VisibleCardProxy : CardProxy
{
    [SerializeField] private TMPro.TMP_Text NameText;
    [SerializeField] private TMPro.TMP_Text DescriptionText;
    [SerializeField] private TMPro.TMP_Text EnergyText;

    protected Vector3 BasePosition { get; set; }

    protected override void OnInitialize()
    {
        base.OnInitialize();
        //Possibly just make new card proxy for selection. Shouldn't listen to events
        if (GameEntity.Context != null)
        {
            GameEntity.Context.Events.CardPlayed += EventsOnCardPlayed;

        }

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

    public virtual void SetBasePosition(Vector3 basePosition)
    {
        BasePosition = basePosition;
        transform.localPosition = BasePosition;
    }

    public int DisplayIndex { get; set; }

}
