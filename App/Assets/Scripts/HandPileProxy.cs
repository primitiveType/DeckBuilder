using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPileProxy : PileProxy<HandCardProxy>
{
    [SerializeField]
    private float CardWidth;
    [SerializeField]
    private float CardDepth;
    [SerializeField]
    private float CardSeperation;

    [SerializeField]
    private Vector3 CenterPosition;

    public IReadOnlyList<HandCardProxy> GetSelectedCards()
    {
        List<HandCardProxy> selectedCards = new List<HandCardProxy>();
        foreach(HandCardProxy cardProxy in CardProxies.Values)
        {
            if(cardProxy.Selected)
            {
                selectedCards.Add(cardProxy);
            }
        }

        return selectedCards;
    }

    protected override void CreateCardProxy(int argsMovedCard)
    {
        base.CreateCardProxy(argsMovedCard);

        int numCards = CardProxies.Count;

        CardProxies[argsMovedCard].HandPositionIndex = numCards - 1;

        OrganizeHand();

    }

    private void OrganizeHand()
    {
        int numCards = CardProxies.Count;

        float cardOffset = ((numCards - 1) * CardWidth) / 2 + ((numCards - 1) * CardSeperation) / 2;

        Vector3 startPosition = cardOffset * Vector3.left + CenterPosition;

        foreach (HandCardProxy card in CardProxies.Values)
        {
            card.ResetHandPosition(card.HandPositionIndex * ((CardWidth + CardSeperation) * Vector3.right) + startPosition + (Vector3.back * CardDepth  * card.HandPositionIndex));
        }

    }

    protected override void DestroyCardProxy(int argsMovedCard)
    {
        HandCardProxy cardProxy = CardProxies[argsMovedCard];
        int position = cardProxy.HandPositionIndex;

        base.DestroyCardProxy(argsMovedCard);

        foreach(HandCardProxy card in CardProxies.Values)
        {
            if(card.HandPositionIndex > position)
            {
                card.HandPositionIndex -= 1;
            }
        }

        OrganizeHand();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            foreach (HandCardProxy handCardProxy in CardProxies.Values)
            {
                handCardProxy.Selected = false;
            }
        }

    }
}
