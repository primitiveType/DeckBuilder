using App;
using SummerJam1.Statuses;
using UnityEngine;

namespace SummerJam1
{
    public class AsleepComponentView : ComponentView<IsTopMonster>
    {

        protected override void ComponentOnPropertyChanged()
        {
            VisibilityObject.SetActive(Component is { Enabled: false });
        }
    }
}
