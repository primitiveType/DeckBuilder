using System.Collections.Specialized;
using Api;
using UnityEngine;

public class DeckPileOrganizer : PileOrganizer
{
    [SerializeField] private Transform parentTransform;


    protected override void ParentViewToPile(IEntity added)
    {
        IGameObject viewGO = added.GetComponent<IGameObject>();

        IPileItemView view = viewGO.gameObject.GetComponent<IPileItemView>();
        viewGO.gameObject.transform.SetParent(parentTransform);
        view.SetLocalPosition(new Vector3(), new Vector3());
    }
}