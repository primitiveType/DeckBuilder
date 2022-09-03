using System.Threading.Tasks;
using Api;
using UnityEngine;

namespace App
{
    public class MessyPileOrganizer : PileOrganizer
    {
        private void Update()
        {
            foreach (IEntity child in Entity.Children)
            {
                // OnItemAddedQueued(child);
            }
        }

        protected override async Task OnItemAddedQueued(IEntity added, IGameObject view1)
        {
            IGameObject view = added.GetComponent<IGameObject>();
            view.gameObject.transform.SetParent(transform);
            IPileItemView pileItemView = view.gameObject.GetComponentInChildren<IPileItemView>();
            pileItemView.SetTargetPosition(view.gameObject.transform.localPosition, Vector3.zero);
        }
    }
}
