using App;
using CardsAndPiles.Components;
using UnityEngine.EventSystems;

namespace SummerJam1
{
    public class CardInPrizePool : View<Card>, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            SummerJam1Context.Instance.Game.PrizePile.ChoosePrize(Entity);
        }

    }
}