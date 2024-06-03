using System.Collections;
using System.Collections.Generic;
using App;
using SummerJam1.Units;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardClickComponent : ComponentView<IClickable>, IPointerClickHandler
{
    protected override bool m_DisableComponentIfNull => true;

    protected override void ComponentOnPropertyChanged()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Component?.Click();
    }
}
