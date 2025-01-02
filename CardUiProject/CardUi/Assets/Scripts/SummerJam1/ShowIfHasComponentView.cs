using System.ComponentModel;
using App;
using IComponent = Api.IComponent;

namespace SummerJam1
{
    public class ShowIfHasComponentView<TComponent> : ComponentView<TComponent> where TComponent : INotifyPropertyChanged, IComponent
    {
        protected override void ComponentOnPropertyChanged()
        {
            bool enable = Component != null;
            Disposables.Add(AnimationQueue.Instance.Enqueue(( ()=>Enable(enable))));
        }

        protected virtual void Enable(bool enable)
        {
            gameObject.SetActive(enable);
        }
    }
}
