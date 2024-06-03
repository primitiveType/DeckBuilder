using System.Threading.Tasks;
using Api;
using CardsAndPiles;
using UnityEngine;

namespace App
{
    public class DeckPileOrganizer : PileOrganizer
    {
        [SerializeField] private bool SetPosition;

        protected override async Task OnItemAddedQueued(IEntity added, IGameObject view)
        {
            await base.OnItemAddedQueued(added, view);

            if (view?.gameObject == null)
            {
                Debug.LogError($"Added game object was null. {added.GetDebugString()}.", this);
            }

            IPileItemView pileItemView = view.gameObject.GetComponentInChildren<IPileItemView>();

            if (SetPosition)
            {
                if (pileItemView != null)
                {
                    pileItemView.SetTargetPosition(new Vector3(), new Vector3());
                }
                else
                {
                    view.gameObject.transform.localPosition = new Vector3();
                    view.gameObject.transform.localRotation = Quaternion.identity;
                }
            }
        }

        // protected override void OnItemAddedImmediate(IEntity added, IGameObject view)
        // {
        //     base.OnItemAddedImmediate(added, view);
        //     view.gameObject.transform.SetParent(m_ParentTransform, true);
        // }
    }
}