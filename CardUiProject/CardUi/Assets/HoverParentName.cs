using Api;
using App;
using CardsAndPiles.Components;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverParentName : ComponentView<IEntity>, IPointerEnterHandler
{

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log(Entity.Parent.GetComponent<NameComponent>().Value);
    }

    protected override void ComponentOnPropertyChanged()
    {
        
    }
}
