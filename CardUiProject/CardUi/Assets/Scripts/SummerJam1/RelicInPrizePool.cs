using SummerJam1;
using UnityEngine.EventSystems;

namespace App
{
    public class RelicInPrizePool : View<RelicComponent>, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            SummerJam1Context.Instance.Game.RelicPrizePile.ChoosePrize(Entity);
        }
    }
}