using System.Linq;
using Api;
using UnityEngine;

public class DeckPileOrganizer : PileOrganizer
{
    [SerializeField] private Transform parentTransform;


    protected override void OnItemAdded(IEntity added)
    {
        
        IGameObject viewGO = added.GetComponent<IGameObject>();
        if (viewGO == null || viewGO.gameObject == null)
        {
            Debug.LogError($"Failed to find game object for entity {added.Id}:{added.GetComponents<IComponent>().FirstOrDefault()?.GetType().Name}.");
        }
        IPileItemView view = viewGO.gameObject.GetComponent<IPileItemView>();
        viewGO.gameObject.transform.SetParent(parentTransform);
        view.SetTargetPosition(new Vector3(), new Vector3());
    }
}