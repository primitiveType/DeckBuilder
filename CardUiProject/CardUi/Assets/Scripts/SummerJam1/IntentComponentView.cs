using SummerJam1;
using UnityEngine;
using UnityEngine.UI;

namespace App
{
    public class IntentComponentView : ComponentView<Intent>
    {
        [SerializeField] private Text AmountText;
        [SerializeField] private Image IntentImage;

        [SerializeField] private Sprite DamageIntentImage;


        protected override void ComponentOnPropertyChanged()
        {
            if (Component is IAmount amount)
            {
                AmountText.gameObject.SetActive(true);
                AmountText.text = amount.Amount.ToString();
            }
            else
            {
                AmountText.gameObject.SetActive(false);
            }

            UpdateIntentImage();
        }

        private void UpdateIntentImage()
        {
            IntentImage.gameObject.SetActive(true);

            switch (Component)
            {
                case DamageIntent _:
                    IntentImage.sprite = DamageIntentImage;
                    break;
                default:
                    IntentImage.gameObject.SetActive(false);
                    break;
            }
        }
    }
}