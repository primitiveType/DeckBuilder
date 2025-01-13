using App;
using UnityEditor;

namespace SummerJam1
{
    public class IntentView : ComponentView<Intent>
    {
        protected override void ComponentOnPropertyChanged()
        {
            VisibilityObject.SetActive(Component.Enabled);
        }
    }
}