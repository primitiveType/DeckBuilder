using System.Linq;
using System.Threading.Tasks;
using Api;
using UnityEngine;

namespace App
{
    public class DeckPileOrganizer : PileOrganizer
    {
        [SerializeField] private Transform m_ParentTransform;
        [SerializeField] private bool SetPosition;

        protected override async Task OnItemAddedQueued(IEntity added)
        {
            await base.OnItemAddedQueued(added);
            IGameObject viewGO = added.GetComponent<IGameObject>();
            if (viewGO == null)
            {
                Debug.LogError(
                    $"Failed to find game object component for entity {added.Id}:{added.GetComponents<IComponent>().FirstOrDefault()?.GetType().Name}.",
                    gameObject);
                return;
            }

            if (viewGO.gameObject == null)
            {
                Debug.LogError(
                    $"Failed to find game object for entity {added.Id}:{added.GetComponents<IComponent>().FirstOrDefault()?.GetType().Name}.",
                    gameObject);
                return;
            }

            IPileItemView view = viewGO.gameObject.GetComponent<IPileItemView>();
            viewGO.gameObject.transform.SetParent(m_ParentTransform);
            if (SetPosition)
            {
                view.SetTargetPosition(new Vector3(), new Vector3());
            }
        }

        protected override void OnItemAddedImmediate(IEntity added)
        {
            base.OnItemAddedImmediate(added);
            IGameObject viewGO = added.GetComponent<IGameObject>();
            if (viewGO == null)
            {
                Debug.LogError(
                    $"Failed to find game object component for entity {added.Id}:{added.GetComponents<IComponent>().FirstOrDefault()?.GetType().Name}.",
                    gameObject);

                return;
            }

            if (viewGO.gameObject == null)
            {
                Debug.LogError(
                    $"Failed to find game object for entity {added.Id}:{added.GetComponents<IComponent>().FirstOrDefault()?.GetType().Name}.",
                    gameObject);
                return;
            }
            
            viewGO.gameObject.transform.SetParent(m_ParentTransform, true);
        }
    }
}
