using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DeckbuilderLibrary.Data.GameEntities;

public class SelectionDisplay : MonoBehaviour
{
    [SerializeField] private float CardWidth;

    [SerializeField] private float CardDepth;

    [SerializeField] private float CardSeperation;

    [SerializeField] private VisibleCardProxy VisualCardProxyPrefab;

    [SerializeField] private Vector3 CenterPosition;

    [SerializeField] private GameObject GreyScreenObject;

    private List<VisibleCardProxy> SelectableCardProxies = new List<VisibleCardProxy>();


    public bool IsDisplaying { get; private set; }

    public void DisplaySelectableCards(IEnumerable<Card> selectableCards)
    {
        IsDisplaying = true;
        GreyScreenObject.SetActive(true);

        foreach (Card card in selectableCards)
        {
            VisibleCardProxy visualCardProxy = Instantiate(VisualCardProxyPrefab, transform);
            visualCardProxy.Initialize(card);
            SelectableCardProxies.Add(visualCardProxy);
        }

        for (int i = 0; i < SelectableCardProxies.Count; i++)
        {
            SelectableCardProxies[i].DisplayIndex = i;
        }

        OrganizeDisplay();
    }

    public void ClearDisplay()
    {
        IsDisplaying = false;
        GreyScreenObject.SetActive(false);
        foreach (VisibleCardProxy visualCardProxy in SelectableCardProxies)
        {
            Destroy(visualCardProxy.gameObject);
        }

        SelectableCardProxies.Clear();
    }

    private void OrganizeDisplay()
    {
        int numCards = SelectableCardProxies.Count;

        float cardOffset = ((numCards - 1) * CardWidth) / 2 + ((numCards - 1) * CardSeperation) / 2;

        Vector3 startPosition = cardOffset * Vector3.left + CenterPosition;

        foreach (VisibleCardProxy card in SelectableCardProxies)
        {
            card.SetBasePosition(card.DisplayIndex * ((CardWidth + CardSeperation) * Vector3.right) +
                                   startPosition + (Vector3.back * CardDepth * card.DisplayIndex));
        }
    }
}