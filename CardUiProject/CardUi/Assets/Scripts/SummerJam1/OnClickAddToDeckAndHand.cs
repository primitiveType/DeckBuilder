using Api;
using App;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SummerJam1
{
    public class OnClickAddToDeckAndHand : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            IEntity entity = GetComponentInChildren<IView>().Entity;
            IEntity copy = (entity.Context.DuplicateEntity(entity));
            IEntity copy2 = (entity.Context.DuplicateEntity(entity));

            copy2.TrySetParent(entity.Context.Root.GetComponent<Game>().Deck.Entity);
            
            copy.TrySetParent(entity.Context.Root.GetComponent<Game>().Battle.Hand.Entity);
        }
    }
}
