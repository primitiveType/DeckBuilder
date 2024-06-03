using System.Threading.Tasks;
using App;
using UnityEngine;

namespace SummerJam1
{
    public class StrengthComponentView : AmountComponentView<Strength>
    {
        private int? _previous;
        
        protected override async Task ValueChanged(int? amount)
        {
            await base.ValueChanged(amount);
            if (amount.HasValue)
            {
                int diff = _previous.HasValue ? amount.Value - _previous.Value : amount.Value;
                if (diff != 0)
                {
                    string sign = diff > 0 ? "+" : "";
                    TextPopupManager.Instance.Add($"{sign}{diff} Strength", ((MonoBehaviour)View).gameObject,
                        TextPopupManager.STAT_TEXT);
                }
            }

            _previous = amount;
        }
    }
}
