using System;
using System.Threading.Tasks;
using App;
using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class MultiAttackComponentView : ComponentView<MultiAttack>
    {
        [SerializeField] private Text AmountText;

        protected override void ComponentOnPropertyChanged()
        {
            string amount = null;
            if (Component != null)
            {
                amount = " X " + (1 + Component.Amount);
            }


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

        private void OnDisable()
        {
            AmountText.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            AmountText.gameObject.SetActive(true);
        }
    }
}