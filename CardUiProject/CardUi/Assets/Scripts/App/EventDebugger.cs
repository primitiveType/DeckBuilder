using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace App
{
    public class EventDebugger : MonoBehaviour, IPointerEnterHandler, IDragHandler
    {
        public void OnPointerEnter(PointerEventData eventData) => Log();

        public void OnDrag(PointerEventData eventData) => Log();

        private void Log([CallerMemberName] string method = null)
        {
            Debug.Log($"{method} on {name}!", this);
        }
    }
}
