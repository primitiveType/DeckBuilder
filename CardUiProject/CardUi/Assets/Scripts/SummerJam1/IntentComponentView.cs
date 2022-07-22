using System.Data;
using Api;
using App;
using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class IntentComponentView : ComponentView<Intent>
    {
        // [SerializeField] private Text AmountText;
        [SerializeField] private Image IntentImage;
        [SerializeField] private Sprite SleepingImage;

        [SerializeField] private Sprite DamageIntentImage;
        // [SerializeField] private GameObject HideIfSleeping;
        [SerializeField] private GameObject ShowIfSleeping;


        protected override void ComponentOnPropertyChanged()
        {
            UpdateIntentImage();
        }

        private void UpdateIntentImage()
        {
            IntentImage.gameObject.SetActive(true);
            // HideIfSleeping.SetActive(Component.Enabled);
            ShowIfSleeping.SetActive(!Component.Enabled);

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
