using App;
using SummerJam1.Cards;
using SummerJam1.Statuses;
using UnityEngine;

namespace SummerJam1
{
    public class AsleepComponentView : ComponentView<Asleep>
    {
        protected override void ComponentOnPropertyChanged()
        {
            bool isEnabled = Component is { Enabled: false };
            Disposables.Add(AnimationQueue.Instance.Enqueue(() => VisibilityObject.SetActive(isEnabled)));
        }
    }
}
