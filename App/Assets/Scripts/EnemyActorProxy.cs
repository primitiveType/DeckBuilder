using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Data;

public class EnemyActorProxy : ActorProxy
{
    private HandPileProxy HandPileProxy { get; set; }

    [SerializeField]
    private Text HealthText;

    [SerializeField]
    private Text ArmorText;

    private void Start()
    {
        //TODO Don't use GameObject.Find 
        HandPileProxy = GameObject.Find("HandPileProxy").GetComponent<HandPileProxy>();
        UpdateText();
    }

    private void OnMouseDown()
    {
        IReadOnlyList<HandCardProxy> SelectedCards = HandPileProxy.GetSelectedCards();
        foreach(HandCardProxy selectedCard in SelectedCards)
        {
            Card card = selectedCard.GameEntity;
            IReadOnlyList<Actor> validTargets = card.GetValidTargets();
            if(validTargets.Contains(GameEntity))
            {
                card.PlayCard(GameEntity);
            }

            selectedCard.Selected = false;
        }
    }

    public override void DamageReceived()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        HealthText.text = $"Health: {GameEntity.Health.ToString()}";
        ArmorText.text = $"Armor: {GameEntity.Armor.ToString()}";
    }
}
