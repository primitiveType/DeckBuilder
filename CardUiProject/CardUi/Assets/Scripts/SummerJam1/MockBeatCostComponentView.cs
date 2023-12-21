using System;
using App;
using SummerJam1.Rules;
using UnityEngine.EventSystems;

namespace SummerJam1
{
    public class MockBeatCostComponentView : ComponentView<WaitForCardCostsBeats>, IPointerEnterHandler, IPointerExitHandler
    {
        private BeatTracker Tracker { get; set; }
        private IDisposable hoverDispose { get; set; }

        protected override bool SearchParents => true;
        
        protected override void Start()
        {
            base.Start();
            Tracker = Entity.Context.Root.GetComponent<Game>().Battle.BeatTracker;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            hoverDispose?.Dispose();
            hoverDispose = Tracker.GetPreview(Component.BeatCost);
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

        protected override void ComponentOnPropertyChanged()
        {
            DisposeHover();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            DisposeHover();
        }

        private void OnDisable()
        {
            DisposeHover();
        }
    }
}