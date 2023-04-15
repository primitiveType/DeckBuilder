using App;
using SummerJam1.Units.Effects;

namespace SummerJam1
{
    public class MultiAttackComponentView : AmountComponentView<MultiAttack>
    {
        protected override string GetStringForAmount(int? amount)
        {
            if (amount == null)
            {
                return "";
            }

            return $" X {amount.Value}";
        }
    }
}
