using UnityEngine;

namespace App
{
    public class CardInHand : MonoBehaviour
    {
        public IPileItemView PileItemView { get; private set; }

        public bool DisplayWholeCard => HoverExpand.IsHovered && enabled && HoverExpand.WarmedUp;
        
        private HoverExpand HoverExpand { get; set; }

        private void Awake()
        {
            HoverExpand = gameObject.AddComponent<HoverExpand>();
            PileItemView = GetComponentInParent<IPileItemView>();
            GetComponent<RectTransform>().sizeDelta = new Vector2(3, 5);
            if (PileItemView == null)
            {
                Debug.LogError("Pile item not found when adding card to hand!");
            }
        }

        private void OnDestroy()
        {
            Destroy(HoverExpand);
        }
    }
}
