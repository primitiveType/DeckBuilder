using System.Threading.Tasks;
using Api;
using App;
using UnityEngine;
using Component = UnityEngine.Component;

namespace SummerJam1
{
    public class PileOrganizerWithComponent<TComponentToAdd> : PileOrganizer where TComponentToAdd : Component
    {
        protected override async Task OnItemAddedQueued(IEntity added, IGameObject view)
        {
            await base.OnItemAddedQueued(added, view);
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
