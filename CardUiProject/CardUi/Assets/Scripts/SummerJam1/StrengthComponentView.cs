using SummerJam1;
using UnityEngine;
using UnityEngine.UI;

namespace App
{
    public class StrengthComponentView : ComponentView<Strength>
    {
        [SerializeField] private Text AmountText;
        protected override void ComponentOnPropertyChanged()
        {
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