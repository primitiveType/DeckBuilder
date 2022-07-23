using System.Collections.Specialized;
using App;

namespace SummerJam1
{
    public class RelicPrizePileView : PileView
    {
        protected virtual void Awake()
        {
            SetModel(GameContext.Instance.Game.RelicPrizePile.Entity);
            Entity.Children.CollectionChanged += ChildrenOnCollectionChanged;
            UpdateVisibility();
        }

        private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            gameObject.SetActive(Entity.Children.Count > 0);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Entity.Children.CollectionChanged -= ChildrenOnCollectionChanged;
        }
    }
}
