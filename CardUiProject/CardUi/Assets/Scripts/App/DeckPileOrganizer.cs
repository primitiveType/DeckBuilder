using System.Linq;
using Api;
using UnityEngine;

public class DeckPileOrganizer : PileOrganizer
{
    [SerializeField] private Transform parentTransform;


    protected override void ParentViewToPile(IEntity added)
    {
        
        IGameObject viewGO = added.GetComponent<IGameObject>();
        if (viewGO == null || viewGO.gameObject == null)
        {
            Debug.LogError($"Failed to find game object for entity {added.Id}:{added.GetComponents<IComponent>().FirstOrDefault()?.GetType().Name}.");
        }
        IPileItemView view = viewGO.gameObject.GetComponent<IPileItemView>();
        viewGO.gameObject.transform.SetParent(parentTransform);
        view.SetLocalPosition(new Vector3(), new Vector3());
    }
}