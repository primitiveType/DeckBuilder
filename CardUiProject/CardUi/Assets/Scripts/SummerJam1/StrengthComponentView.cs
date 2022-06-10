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
            AmountText.text = Component.Amount.ToString();
        }
    }
}