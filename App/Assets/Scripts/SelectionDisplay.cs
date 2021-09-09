using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeckbuilderLibrary.Data.GameEntities;

public class SelectionDisplay : MonoBehaviour
{
    [SerializeField] private float CardWidth;

    [SerializeField] private float CardDepth;

    [SerializeField] private float CardSeperation;

    [SerializeField] private HandCardProxy HandCardProxyPrefab;

    [SerializeField] private Vector3 CenterPosition;

    [SerializeField] private GameObject GreyScreenObject;

    private List<HandCardProxy> SelectableCardProxies = new List<HandCardProxy>();


    public bool IsDisplaying { get; private set; }

    public void DisplaySelectableCards(IEnumerable<Card> selectableCards)
    {
        IsDisplaying = true;
        GreyScreenObject.SetActive(true);

        foreach (Card card in selectableCards)
        {
            HandCardProxy handCardProxy = Instantiate(HandCardProxyPrefab);
            handCardProxy.Initialize(card);
            SelectableCardProxies.Add(handCardProxy);
        }

        for (int i = 0; i < SelectableCardProxies.Count; i++)
        {
            SelectableCardProxies[i].HandPositionIndex = i;
        }

        OrganizeDisplay();
    }

    public void ClearDisplay()
    {
        IsDisplaying = false;
        GreyScreenObject.SetActive(false);
        foreach (HandCardProxy handCardProxy in SelectableCardProxies)
        {
            Destroy(handCardProxy.gameObject);
        }

        SelectableCardProxies.Clear();
    }

    private void OrganizeDisplay()
    {
        int numCards = SelectableCardProxies.Count;

        float cardOffset = ((numCards - 1) * CardWidth) / 2 + ((numCards - 1) * CardSeperation) / 2;

        Vector3 startPosition = cardOffset * Vector3.left + CenterPosition;

        foreach (HandCardProxy card in SelectableCardProxies)
        {
            card.ResetHandPosition(card.HandPositionIndex * ((CardWidth + CardSeperation) * Vector3.right) +
                                   startPosition + (Vector3.back * CardDepth * card.HandPositionIndex));
        }
    }
}