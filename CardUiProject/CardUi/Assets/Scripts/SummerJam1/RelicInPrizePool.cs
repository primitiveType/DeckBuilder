using App;
using UnityEngine.EventSystems;

namespace SummerJam1
{
    public class RelicInPrizePool : View<RelicComponent>, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            SummerJam1Context.Instance.Game.RelicPrizePile.ChoosePrize(Entity);
        }
    }
}