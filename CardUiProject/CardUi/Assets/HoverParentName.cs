using Api;
using App;
using CardsAndPiles.Components;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverParentName : ComponentView<IEntity>, IPointerEnterHandler, IPointerExitHandler
{

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Entity.Parent.GetOrAddComponent<Focused>();
    }

    protected override void ComponentOnPropertyChanged()
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Entity.Parent.RemoveComponent<Focused>();
    }
}
