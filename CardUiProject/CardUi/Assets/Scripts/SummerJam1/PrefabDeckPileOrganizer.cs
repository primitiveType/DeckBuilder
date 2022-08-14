using Api;
using App;

namespace SummerJam1
{
    public class PrefabDeckPileOrganizer : DeckPileOrganizer
    {
        protected override void OnItemAddedImmediate(IEntity added, IGameObject view)
        {
            base.OnItemAddedImmediate(added, view);
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