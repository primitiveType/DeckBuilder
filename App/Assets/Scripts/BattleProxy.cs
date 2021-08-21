using System;
using Data;
using DeckbuilderLibrary.Data;
using UnityEngine;
using UnityEngine.UI;

public class BattleProxy : Proxy<IBattle>
{
    //This class should probably just be the DrawPileProxy. It was my first stab at working with the UI and didn't have it figured out yet. 

    [SerializeField] private Text DrawPileCount;

    [SerializeField] private Button DrawCardButton;

    [SerializeField] private HandPileProxy handPileProxy;

    [SerializeField] private GameObject VictoryGameObject;
    [SerializeField] private GameObject LossGameObject;


    private IContext Api => GameEntity.Context;
    private IGameEventHandler GameEventHandler => Api.Events;

    private IBattle CurrentBattle { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        GameEventHandler.CardMoved += OnCardMoved;
        GameEventHandler.BattleEnded += OnBattleEnded;
        CurrentBattle = Api.GetCurrentBattle();
        DrawCardButton.onClick.AddListener(DrawCardButtonClicked);

        DrawPileCount.text = $"Draw Pile: {CurrentBattle.Deck.DrawPile.Cards.Count.ToString()}";
    }

    private void OnBattleEnded(object sender, BattleEndedEventArgs args)
    {
        VictoryGameObject.SetActive(args.IsVictory);
        LossGameObject.SetActive(!args.IsVictory);
    }

    private void OnCardMoved(object sender, CardMovedEventArgs args)
    {
        if (args.PreviousPileType == PileType.DrawPile || args.NewPileType == PileType.DrawPile)
        {
            DrawPileCount.text = $"Draw Pile: {CurrentBattle.Deck.DrawPile.Cards.Count.ToString()}";
        }
    }

    private void DrawCardButtonClicked()
    {
        //TODO Also check if hand has enough space for another card
        if (CurrentBattle.Deck.DrawPile.Cards.Count > 0)
        {
            CurrentBattle.Deck.SendToPile(CurrentBattle.Deck.DrawPile.Cards[0], PileType.HandPile);
        }
    }


    protected override void OnInitialize()
    {
    }
}