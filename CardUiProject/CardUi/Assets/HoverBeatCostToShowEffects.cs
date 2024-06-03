using App;
using SummerJam1.Cards;
using UnityEngine.EventSystems;

public class HoverBeatCostToShowEffects : ComponentView<BeatCost>, IPointerEnterHandler, IPointerExitHandler
{
    //TODO:
    //I think hovering a card should show everything on the board that will change.
    //On the sim side this should be as simple as duplicating the context, and playing the card in that context.
    //On the vis side, we must then compare the two contexts. This means that all views will have to be capable of doing so...
    //It also means we need some way of signaling to all views that they should do so.
    protected override void ComponentOnPropertyChanged()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
