using App;
using SummerJam1.Units;
using UnityEngine.EventSystems;

namespace SummerJam1
{
    public class HoverFocusesIntents : ComponentView<StarterUnit>, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            foreach (Intent intent in Entity.GetComponentsInChildren<Intent>())
            {
                intent.Entity.GetOrAddComponent<Focused>();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            foreach (Intent intent in Entity.GetComponentsInChildren<Intent>())
            {
                intent.Entity.RemoveComponent<Focused>();
            }
        }

        protected override void ComponentOnPropertyChanged() { }
    }
}
