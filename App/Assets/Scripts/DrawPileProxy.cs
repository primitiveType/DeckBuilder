using Data;
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

    private IContext Api => GameEntity.Context;
    private IGameEventHandler GameEventHandler => Api.Events;

    private Battle CurrentBattle => Api.GetCurrentBattle();

    // Start is called before the first frame update
    void Start()
    {
        DrawCardButton.onClick.AddListener(DrawCardButtonClicked);

        SetText();
    }
    private void DrawCardButtonClicked()
    {
        //TODO Also check if hand has enough space for another card
        if (CurrentBattle.Deck.DrawPile.Cards.Count > 0)
        {
            CurrentBattle.Deck.SendToPile(CurrentBattle.Deck.DrawPile.Cards[0], PileType.HandPile);
        }
    }
    protected override void GameEventHandlerOnCardMoved(object sender, CardMovedEventArgs args)
    {
        base.GameEventHandlerOnCardMoved(sender, args);

        if (args.PreviousPileType == PileType.DrawPile || args.NewPileType == PileType.DrawPile)
        {
            SetText();
        }

    }

    private void SetText()
    {
        DrawPileCount.text = $"Draw Pile: {CurrentBattle.Deck.DrawPile.Cards.Count.ToString()}";
    }




}
