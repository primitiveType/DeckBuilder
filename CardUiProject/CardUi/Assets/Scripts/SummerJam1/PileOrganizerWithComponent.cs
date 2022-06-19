using Api;
using UnityEngine;
using Component = UnityEngine.Component;

namespace App
{
    public class PileOrganizerWithComponent<TComponentToAdd> : PileOrganizer where TComponentToAdd : Component
    {
        protected override void OnItemAddedQueued(IEntity added)
        {
            base.OnItemAddedQueued(added);
            GameObject entityGO = added.GetComponent<IGameObject>()?.gameObject;
            if (entityGO != null)
            {
                entityGO.AddComponent<TComponentToAdd>();
            }
        }

        protected override void OnItemRemovedQueued(IEntity removed)
        {
            base.OnItemRemovedQueued(removed);
            GameObject entityGO = removed.GetComponent<IGameObject>()?.gameObject;
            if (entityGO != null)
            {
                var com = entityGO.GetComponent<TComponentToAdd>();
                Destroy(com);
            }
        }
    }
}
