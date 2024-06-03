using System;
using Api;
using App;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SummerJam1
{
    public class OnClickDestroy : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            IEntity entity = GetComponentInChildren<IView>().Entity;
            entity.Destroy();
        }
    }
    
}
