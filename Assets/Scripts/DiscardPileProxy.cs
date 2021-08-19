using System;
using UnityEngine;
using UnityEngine.UI;

public class DiscardPileProxy : PileProxy<CardProxy>
{
    [SerializeField]
    private Text DiscardPileCount;

    [SerializeField]
    private Button SendToDrawButon;

    private IGlobalApi Api => Injector.GlobalApi;
    private Battle CurrentBattle { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        CurrentBattle = Api.GetCurrentBattle();
        SetText();

        SendToDrawButon.onClick.AddListener(OnDrawButtonClicked);
    }

    private void OnDrawButtonClicked()
    {
        for(int i = 0; i < GameEntity.Count; i++)
        {
            CurrentBattle.Deck.SendToPile(GameEntity[i], CardPile.DrawPile);
        }
    }

    protected override void GameEventHandlerOnCardMoved(object sender, CardMovedEventArgs args)
    {
        base.GameEventHandlerOnCardMoved(sender, args);

        if (args.PreviousPile == CardPile.DiscardPile || args.NewPile == CardPile.DiscardPile)
        {
            SetText();
        }

    }

    private void SetText()
    {
        DiscardPileCount.text = $"Discard Pile: {CurrentBattle.Deck.DiscardPile.Count.ToString()}";
    }

}
