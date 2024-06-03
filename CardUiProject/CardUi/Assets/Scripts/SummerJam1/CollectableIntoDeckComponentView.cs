using Api;
using App;
using SummerJam1.Units;
using UnityEngine.EventSystems;

namespace SummerJam1
{
    public class CollectableIntoDeckComponentView : View<CollectableIntoDeck>, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            Entity.TrySetParent(Entity.Context.Root.GetComponent<Game>().Battle.BattleDeck.Entity);
        }
        
    }
}
