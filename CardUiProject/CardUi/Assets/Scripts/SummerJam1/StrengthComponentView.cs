using App;
using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class StrengthComponentView : ComponentView<Strength>
    {
        [SerializeField] private Text AmountText;
        protected override void ComponentOnPropertyChanged()
        {
            if (this == null)
            {
                return;
            }
            if (Component == null)
            {
                enabled = false;
                return;
            }

            enabled = true;
            AmountText.text = Component.Amount.ToString();
        }
    }
}