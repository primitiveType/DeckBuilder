using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DeckbuilderLibrary.Data.GameEntities;

public class HandPileProxy : PileProxy<HandCardProxy>
{
    [SerializeField] private float CardWidth;
    [SerializeField] private float CardDepth;
    [SerializeField] private float CardSeperation;

    [SerializeField] private Vector3 CenterPosition;

    public IReadOnlyList<HandCardProxy> GetSelectedCards()
    {
        List<HandCardProxy> selectedCards = new List<HandCardProxy>();
        foreach (HandCardProxy cardProxy in CardProxies.Values)
        {
            if (cardProxy.Selected)
            {
                selectedCards.Add(cardProxy);
            }
        }

        return selectedCards;
    }

    public bool TryGetCardById(int id, out HandCardProxy handCardProxy)
    {
        handCardProxy = CardProxies.Values.FirstOrDefault(x => x.GameEntity.Id == id);
        return handCardProxy != null;
    }

    protected override Proxy<Card> CreateCardProxy(int argsMovedCard)
    {
        var proxy = base.CreateCardProxy(argsMovedCard);

        int numCards = CardProxies.Count;

        CardProxies[argsMovedCard].DisplayIndex = numCards - 1;

        OrganizeHand();

        return proxy;
    }

    private void OrganizeHand()
    {
        int numCards = CardProxies.Count;

        float cardOffset = ((numCards - 1) * CardWidth) / 2 + ((numCards - 1) * CardSeperation) / 2;

        Vector3 startPosition = cardOffset * Vector3.left + CenterPosition;

        foreach (HandCardProxy card in CardProxies.Values)
        {
            card.ResetHandPosition(card.DisplayIndex * ((CardWidth + CardSeperation) * Vector3.right) +
                                   startPosition + (Vector3.back * CardDepth * card.DisplayIndex));
        }
    }

    protected override void DestroyCardProxy(int argsMovedCard)
    {
        HandCardProxy cardProxy = CardProxies[argsMovedCard];
        int position = cardProxy.DisplayIndex;

        base.DestroyCardProxy(argsMovedCard);

        foreach (HandCardProxy card in CardProxies.Values)
        {
            if (card.DisplayIndex > position)
            {
                card.DisplayIndex -= 1;
            }
        }

        OrganizeHand();
    }
}