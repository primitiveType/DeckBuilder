using System.Collections.Generic;
using System.Threading.Tasks;
using Api;
using App;
using SummerJam1;
using UnityEngine;

public class ScrollPileOrganizer : PileOrganizer
{
    [SerializeField] private CardUiContainer m_ContainerPrefab;
    [SerializeField] private Transform m_ContainerParent;

    private Dictionary<IEntity, CardUiContainer> CreatedContainers { get; } = new Dictionary<IEntity, CardUiContainer>();

    protected override void OnItemAddedImmediate(IEntity added)
    {
        base.OnItemAddedImmediate(added);
        CardUiContainer container = Instantiate(m_ContainerPrefab, m_ContainerParent);
        container.SetModel(added);
        GameObject card = Instantiate(ViewFactory.Instance.CardPrefab);
        card.GetComponentInChildren<ISetModel>().SetModel(added);
        var pos = container.transform.localPosition;
        container.transform.localPosition = new Vector3(pos.x, pos.y, -1);
        container.AddCard(card);
    }

    protected override void OnItemRemovedImmediate(IEntity removed)
    {
        base.OnItemRemovedImmediate(removed);
        if (CreatedContainers.TryGetValue(removed, out CardUiContainer container))
        {
            Destroy(container.gameObject);
        }
        else
        {
            Debug.LogWarning($"Failed to destroy card container for entity {removed.Id}.");
        }
    }

    protected override async Task OnItemAddedQueued(IEntity added)
    {
        //do nothing.
    }

    protected override void OnItemRemovedQueued(IEntity removed)
    {
        //do nothing.
    }
}
