using System.Linq;
using System.Threading.Tasks;
using Api;
using UnityEngine;

namespace App
{
 
    public class DeckPileOrganizer : PileOrganizer
    {
        [SerializeField] private bool SetPosition;

        protected override async Task OnItemAddedQueued(IEntity added, IGameObject view)
        {
            await base.OnItemAddedQueued(added, view);


            IPileItemView pileView = view.gameObject.GetComponent<IPileItemView>();

            if (SetPosition)
            {
                pileView.SetTargetPosition(new Vector3(), new Vector3());
            }
        }

        // protected override void OnItemAddedImmediate(IEntity added, IGameObject view)
        // {
        //     base.OnItemAddedImmediate(added, view);
        //     view.gameObject.transform.SetParent(m_ParentTransform, true);
        // }
    }
}
