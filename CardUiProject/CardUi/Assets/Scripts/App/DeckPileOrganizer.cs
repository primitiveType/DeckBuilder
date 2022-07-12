using System.Linq;
using Api;
using UnityEngine;

namespace App
{
    public class DeckPileOrganizer : PileOrganizer
    {
        [SerializeField] private Transform m_ParentTransform;


        protected override void OnItemAddedQueued(IEntity added)
        {
            IGameObject viewGO = added.GetComponent<IGameObject>();
            if (viewGO == null)
            {
                Debug.LogError(
                    $"Failed to find game object component for entity {added.Id}:{added.GetComponents<IComponent>().FirstOrDefault()?.GetType().Name}.", gameObject);
                return;
            }

            if (viewGO.gameObject == null)
            {
                Debug.LogError(
                    $"Failed to find game object for entity {added.Id}:{added.GetComponents<IComponent>().FirstOrDefault()?.GetType().Name}.", gameObject);
                return;
            }

            IPileItemView view = viewGO.gameObject.GetComponent<IPileItemView>();
            viewGO.gameObject.transform.SetParent(m_ParentTransform);
            view.SetTargetPosition(new Vector3(), new Vector3());
        }

        protected override void OnItemAddedImmediate(IEntity added)
        {
            base.OnItemAddedImmediate(added);
            IGameObject viewGO = added.GetComponent<IGameObject>();
            if (viewGO == null)
            {
                Debug.LogError(
                    $"Failed to find game object component for entity {added.Id}:{added.GetComponents<IComponent>().FirstOrDefault()?.GetType().Name}.", gameObject);
                return;
            }

            if (viewGO.gameObject == null)
            {
                Debug.LogError(
                    $"Failed to find game object for entity {added.Id}:{added.GetComponents<IComponent>().FirstOrDefault()?.GetType().Name}.", gameObject);
                return;
            }

            viewGO.gameObject.transform.SetParent(m_ParentTransform, true);
        }
    }
}
