using SummerJam1;

namespace App
{
    public class MoneyComponentView : AmountComponentView<Money>
    {
        protected override bool SearchParents => true;
    }
}
