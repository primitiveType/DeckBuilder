using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Battles;
using UnityEngine;
using UnityEngine.UI;

public class DiscardPileProxy : PileProxy<CardProxy>
{
    [SerializeField] private Text DiscardPileCount;

    [SerializeField] private Button SendToDrawButon;

    private IContext Api => GameEntity.Context;
    private IBattle CurrentBattle { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        CurrentBattle = Api.GetCurrentBattle();
        SetText();

        SendToDrawButon.onClick.AddListener(OnDrawButtonClicked);
    }

    private void OnDrawButtonClicked()
    {
        for (int i = GameEntity.Cards.Count - 1; i >= 0; i--)
        {
            CurrentBattle.Deck.TrySendToPile(GameEntity.Cards[i], PileType.DrawPile);
        }
    }

    protected override void GameEventHandlerOnCardMoved(object sender, CardMovedEventArgs args)
    {
        base.GameEventHandlerOnCardMoved(sender, args);

        if (args.PreviousPileType == PileType.DiscardPile || args.NewPileType == PileType.DiscardPile)
        {
            SetText();
        }
    }

    private void SetText()
    {
        DiscardPileCount.text = $"Discard Pile: {CurrentBattle.Deck.DiscardPile.Cards.Count.ToString()}";
    }
}