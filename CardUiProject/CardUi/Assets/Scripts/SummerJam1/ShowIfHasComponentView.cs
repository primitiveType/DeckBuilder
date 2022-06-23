using System.ComponentModel;
using App;

namespace SummerJam1
{
    public class ShowIfHasComponentView<TComponent> : ComponentView<TComponent> where TComponent : INotifyPropertyChanged
    {
        protected override void ComponentOnPropertyChanged()
        {
            bool enable = Component != null;
            Disposables.Add(AnimationQueue.Instance.Enqueue(( ()=>Enable(enable))));
        }

        private void Enable(bool enable)
        {
            gameObject.SetActive(enable);
        }
    }
}