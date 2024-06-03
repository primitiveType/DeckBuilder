using System;
using App;
using SummerJam1.Cards;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SummerJam1
{
    public class BeatCostComponentView : AmountComponentView<BeatCost>, IPointerEnterHandler, IPointerExitHandler
    {
        private BeatTracker Tracker { get; set; }
        private IDisposable hoverDispose { get; set; }
        
        protected override void Start()
        {
            base.Start();
            Tracker = Entity.Context.Root.GetComponent<Game>().Battle.BeatTracker;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hoverDispose?.Dispose();

            hoverDispose = Tracker.GetPreview(Component.Amount);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            DisposeHover();
        }

        private void DisposeHover()
        {
            hoverDispose?.Dispose();
            hoverDispose = null;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DisposeHover();
        }

        private void OnDisable()
        {
            Debug.Log("Disabling it!");
            DisposeHover();
        }
    }
}