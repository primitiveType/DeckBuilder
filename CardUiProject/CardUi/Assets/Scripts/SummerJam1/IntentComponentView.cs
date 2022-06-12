using App;
using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class IntentComponentView : ComponentView<Intent>
    {
        [SerializeField] private Text AmountText;
        [SerializeField] private Image IntentImage;

        [SerializeField] private Sprite DamageIntentImage;


        protected override void ComponentOnPropertyChanged()
        {
            if (Component is IAmount)
            {
                AmountText.gameObject.SetActive(true);
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