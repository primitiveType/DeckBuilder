using UnityEngine;
using UnityEngine.UI;

public class ExhaustPileProxy : PileProxy<CardProxy>
{
    [SerializeField]
    private Text ExhaustPileCount;

    private IGlobalApi Api => Injector.GlobalApi;
    private Battle CurrentBattle { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        CurrentBattle = Api.GetCurrentBattle();
        SetText();
    }

    protected override void GameEventHandlerOnCardMoved(object sender, CardMovedEventArgs args)
    {
        base.GameEventHandlerOnCardMoved(sender, args);

        if (args.PreviousPile == CardPile.ExhaustPile || args.NewPile == CardPile.ExhaustPile)
        {
            SetText();
        }

    }

    private void SetText()
    {
        ExhaustPileCount.text = $"Exhaust Pile: {CurrentBattle.Deck.ExhaustPile.Count.ToString()}";
    }

}
