using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawPileProxy : PileProxy<CardProxy>
{
    [SerializeField]
    private Text DrawPileCount;

    [SerializeField]
    private Button DrawCardButton;

    [SerializeField]
    private HandPileProxy handPileProxy;

    private IGlobalApi Api => Injector.GlobalApi;
    private IGameEventHandler GameEventHandler => Injector.GameEventHandler;

    private Battle CurrentBattle { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        CurrentBattle = Api.GetCurrentBattle();
        DrawCardButton.onClick.AddListener(DrawCardButtonClicked);

        SetText();
    }
    private void DrawCardButtonClicked()
    {
        //TODO Also check if hand has enough space for another card
        if (CurrentBattle.Deck.DrawPile.Count > 0)
        {
            CurrentBattle.Deck.SendToPile(CurrentBattle.Deck.DrawPile[0], CardPile.HandPile);
        }
    }
    protected override void GameEventHandlerOnCardMoved(object sender, CardMovedEventArgs args)
    {
        base.GameEventHandlerOnCardMoved(sender, args);

        if (args.PreviousPile == CardPile.DrawPile || args.NewPile == CardPile.DrawPile)
        {
            SetText();
        }

    }

    private void SetText()
    {
        DrawPileCount.text = $"Draw Pile: {CurrentBattle.Deck.DrawPile.Count.ToString()}";
    }




}
