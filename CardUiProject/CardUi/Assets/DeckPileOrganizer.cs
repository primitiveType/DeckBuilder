using System.Collections.Specialized;
using Api;
using UnityEngine;

public class DeckPileOrganizer : PileOrganizer
{
    [SerializeField] private Transform parentTransform;


    protected override void ParentViewToPile(Entity added)
    {
        IGameObject viewGO = added.GetComponent<IGameObject>();
        
        PileItemView view = viewGO.gameObject.GetComponent<PileItemView>();
        view.gameObject.transform.SetParent(parentTransform);
        view.SetLocalPosition(new Vector3(), new Vector3());
    }
}