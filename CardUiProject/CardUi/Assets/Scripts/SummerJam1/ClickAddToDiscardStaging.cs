using App;
using CardsAndPiles.Components;
using SummerJam1;
using UnityEngine.EventSystems;

public class ClickAddToDiscardStaging : View<Card>, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Entity.Parent == SummerJam1Context.Instance.Game.DiscardStagingPile.Entity)
        {
            Entity.TrySetParent(SummerJam1Context.Instance.Game.Battle.Hand.Entity); //put back in hand.
        }
        else
        {
            Entity.TrySetParent(SummerJam1Context.Instance.Game.DiscardStagingPile.Entity); //put back in hand.
        }
    }
}