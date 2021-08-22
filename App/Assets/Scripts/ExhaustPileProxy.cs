using Data;
using UnityEngine;
using UnityEngine.UI;

public class ExhaustPileProxy : PileProxy<CardProxy>
{
    [SerializeField]
    private Text ExhaustPileCount;

    private IContext Context => GameEntity.Context;
    private IBattle CurrentBattle { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        CurrentBattle = Context.GetCurrentBattle();
        SetText();
    }

    protected override void GameEventHandlerOnCardMoved(object sender, CardMovedEventArgs args)
    {
        base.GameEventHandlerOnCardMoved(sender, args);

        if (args.PreviousPileType == PileType.ExhaustPile || args.NewPileType == PileType.ExhaustPile)
        {
            SetText();
        }

    }

    private void SetText()
    {
        ExhaustPileCount.text = $"Exhaust Pile: {CurrentBattle.Deck.ExhaustPile.Cards.Count.ToString()}";
    }

}
