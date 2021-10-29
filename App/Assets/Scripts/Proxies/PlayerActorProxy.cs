using System;
using System.Runtime.Remoting.Contexts;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActorProxy : ActorProxy<PlayerActor>
{
    [SerializeField] private Text m_HealthText;
    private Text HealthText => m_HealthText;
    [SerializeField] private Text m_EnergyText;
    private Text EnergyText => m_EnergyText;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        UpdateText();


        GameEntity.Context.Events.CardPlayed += OnCardPlayed;
        GameEntity.Context.Events.TurnEnded += OnTurnEnded; //TODO: turn began?
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameEntity.Context.Events.CardPlayed -= OnCardPlayed;
        GameEntity.Context.Events.TurnEnded -= OnTurnEnded; //TODO: turn began?
    }

    private void OnTurnEnded(object sender, TurnEndedEventArgs args)
    {
        UpdateText();
    }

    private void OnCardPlayed(object sender, CardPlayedEventArgs args)
    {
        UpdateText();
    }

    public override void DamageReceived()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        HealthText.text = $"Health : {GameEntity.Health}";
        EnergyText.text = $"Energy : {GameEntity.CurrentEnergy} / {GameEntity.BaseEnergy}";
    }

    
}