using Api;
using UnityEngine;

namespace App
{
    public class MessyPileOrganizer : PileOrganizer
    {
        private void Update()
        {
            foreach (IEntity child in PileView.Entity.Children)
            {
                // OnItemAddedQueued(child);
            }
        }

        protected override void OnItemAddedQueued(IEntity added)
        {
            IGameObject view = added.GetComponent<IGameObject>();
            view.gameObject.transform.SetParent(transform);
            IPileItemView pileItemView = view.gameObject.GetComponentInChildren<IPileItemView>();
            pileItemView.SetTargetPosition(view.gameObject.transform.localPosition, Vector3.zero);
        }
    }
}
