using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.GameEntities;


public class DiscoverPileProxy : PileProxy<DiscoverCardProxy>
{
    [SerializeField] private float CardWidth;
    [SerializeField] private float CardDepth;
    [SerializeField] private float CardSeperation;

    [SerializeField] private Vector3 CenterPosition;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        //  GameEntity.Context.Events.
    }

    protected override DiscoverCardProxy CreateCardProxy(int argsMovedCard)
    {
        var proxy = base.CreateCardProxy(argsMovedCard);
        int numCards = CardProxies.Count;


        if (numCards == 1)
        {
            InputManager.Instance.TransitionToState(InputState.DiscoveringCard);
        }

        CardProxies[argsMovedCard].DisplayIndex = numCards;

        OrganizeHand();

        return proxy;
    }

    protected override void GameEntityOnDestroyedEvent(object sender, EntityDestroyedArgs args)
    {
        base.GameEntityOnDestroyedEvent(sender, args);
        OrganizeHand();
    }

    private void OrganizeHand()
    {
        int numCards = CardProxies.Count;

        float cardOffset = ((numCards - 1) * CardWidth) / 2 + ((numCards - 1) * CardSeperation) / 2;

        Vector3 startPosition = cardOffset * Vector3.left + CenterPosition;


        foreach (DiscoverCardProxy card in CardProxies.Values)
        {
            card.SetBasePosition(card.DisplayIndex * ((CardWidth + CardSeperation) * Vector3.right) +
                                 startPosition + (Vector3.back * CardDepth * card.DisplayIndex));
        }
    }

    public void ClearSelection()
    {
        while (GameEntity.Cards.Count > 0)
        {
            Card card = GameEntity.Cards.First<Card>();
            card.Context.GetCurrentBattle().RemoveCard(card);
        }
    }

    protected override void DestroyCardProxy(int argsMovedCard)
    {
        DiscoverCardProxy cardProxy = CardProxies[argsMovedCard];
        int position = cardProxy.DisplayIndex;

        base.DestroyCardProxy(argsMovedCard);

        foreach (DiscoverCardProxy card in CardProxies.Values)
        {
            if (card.DisplayIndex > position)
            {
                card.DisplayIndex -= 1;
            }
        }

        OrganizeHand();
    }
}