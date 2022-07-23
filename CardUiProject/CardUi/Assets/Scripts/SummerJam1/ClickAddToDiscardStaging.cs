using App;
using CardsAndPiles.Components;
using SummerJam1;
using UnityEngine.EventSystems;

public class ClickAddToDiscardStaging : View<Card>, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if (Entity.Parent == GameContext.Instance.Game.DiscardStagingPile.Entity)
        {
            Entity.TrySetParent(GameContext.Instance.Game.Battle.Hand.Entity); //put back in hand.
        }
        else
        {
            Entity.TrySetParent(GameContext.Instance.Game.DiscardStagingPile.Entity); //put back in hand.
        }
    }
}