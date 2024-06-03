using App;
using SummerJam1.Statuses;

namespace SummerJam1
{
    public class OnlyPlayerSelfDamageDamagesMeComponentView : ComponentView<OnlyPlayerSelfDamageDamagesMe>
    {
        protected override void ComponentOnPropertyChanged()
        {
            gameObject.SetActive(Component != null);
        }
    }
}