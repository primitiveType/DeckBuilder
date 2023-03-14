using App.Utility;
using CardsAndPiles.Components;
using UnityEngine;
using UnityEngine.EventSystems;

namespace App
{
    public class CardPositionComponentView : ComponentView<Position>, IBeginDragHandler, IEndDragHandler
    {
        private bool IsDragging { get; set; }

        private IPileItemView PileItemView { get; set; }

        protected override void Awake()
        {
            base.Awake();
            PileItemView = GetComponent<IPileItemView>();
        }

        private void Update()
        {
            if (IsDragging)
            {
                Component.Pos = PileItemView.GetLocalPosition().ToSystemVector3();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            IsDragging = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            IsDragging = false;
        }

        protected override void ComponentOnPropertyChanged()
        {
            PileItemView.SetTargetPosition(Component.Pos.ToUnityVector3(), Vector3.zero);
        }
    }
}
