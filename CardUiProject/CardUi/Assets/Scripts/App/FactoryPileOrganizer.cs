using System.Threading.Tasks;
using Api;
using UnityEngine;

namespace App
{
    public class FactoryPileOrganizer<T> : PileOrganizer where T : IComponent
    {
        [SerializeField] protected GameObject Prefab;
        protected override bool RequireChildView => false;

        protected override void OnItemAddedImmediate(IEntity added, IGameObject view)
        {
            base.OnItemAddedImmediate(added, view);
            T component = added.GetComponent<T>();
            if (component == null)
            {
                return;
            }

            ISetModel entityView = Instantiate(Prefab, Parent).GetComponentInChildren<ISetModel>();
            entityView.SetModel(component);
        }

        protected override Task OnItemAddedQueued(IEntity added, IGameObject view)
        {
            return Task.CompletedTask; //do nothing, don't call base.
        }
    }
}
