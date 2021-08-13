using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{

    [SerializeField]
    private Text DrawPileCount;

    [SerializeField]
    private Button DrawCardButton;

    private IGlobalApi Api => Injector.GlobalApi;
    private IGameEventHandler GameEventHandler => Injector.GameEventHandler;

    private Battle CurrentBattle { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        GameEventHandler.CardMoved += OnCardMoved;
        CurrentBattle = Api.GetCurrentBattle();
        DrawCardButton.onClick.AddListener(DrawCardButtonClicked);

        DrawPileCount.text = $"Draw Pile: {CurrentBattle.Deck.DrawPile.Count.ToString()}";
    }

    private void OnCardMoved(object sender, CardMovedEventArgs args)
    {
      if(args.PreviousPile == CardPile.DrawPile || args.NewPile == CardPile.DrawPile)
        {
            DrawPileCount.text = $"Draw Pile: {CurrentBattle.Deck.DrawPile.Count.ToString()}";
        }
    }

    private void DrawCardButtonClicked()
    {
        //TODO Also check if hand has enough space for another card
        if(CurrentBattle.Deck.DrawPile.Count > 0)
        {
            CurrentBattle.Deck.SendToPile(CurrentBattle.Deck.DrawPile[0], CardPile.HandPile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
