using System.Threading.Tasks;
using App;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class StrengthComponentView : ComponentView<Strength>
    {
        [SerializeField] private TMP_Text AmountText;

        protected override void ComponentOnPropertyChanged()
        {
            var amount = Component?.Amount.ToString();
            AnimationQueue.Instance.Enqueue(() => UpdateText(amount));
           
        }

        private async Task UpdateText(string text)
        {
            if (this == null)
            {
                return;
            }

            if (text == null)
            {
                enabled = false;
                return;
            }

            enabled = true;
            AmountText.text = text;
        }
    }
}
