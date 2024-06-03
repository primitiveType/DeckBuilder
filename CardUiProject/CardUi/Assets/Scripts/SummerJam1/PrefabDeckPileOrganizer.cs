using System.Threading.Tasks;
using Api;
using App;

namespace SummerJam1
{
    public class PrefabDeckPileOrganizer : DeckPileOrganizer
    {
        protected override async Task OnItemAddedQueued(IEntity added, IGameObject view)
        {
            await base.OnItemAddedQueued(added, view);
            view.gameObject.AddComponent<OnClickAddToDeckAndHand>();
        }

        protected override void OnItemRemovedImmediate(IEntity removed)
        {
            base.OnItemRemovedImmediate(removed);
            var components = removed.GetComponent<IGameObject>().gameObject.GetComponents<OnClickAddToDeckAndHand>();
            foreach (OnClickAddToDeckAndHand onClickAddToDeckAndHand in components)
            {
                Destroy(onClickAddToDeckAndHand);
            }
        }
    }
}
