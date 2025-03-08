using System.Data;
using Api;
using App;
using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class IntentComponentView : View<Intent>
    {
        // [SerializeField] private Text AmountText;
        [SerializeField] private Image IntentImage;
        [SerializeField] private Sprite SleepingImage;

        [SerializeField] private Sprite DamageIntentImage;
        [SerializeField] private Sprite ShieldIntentImage;
        [SerializeField] private GameObject ShowIfSleeping;


        [PropertyListener]
        private void UpdateIntentImage()
        {
            var enabled = Model.Enabled;
            Disposables.Add(AnimationQueue.Instance.Enqueue(() => gameObject.SetActive(enabled)));

            
            IntentImage.gameObject.SetActive(true);

            switch (Model)
            {
                case DamageIntent _:
                    IntentImage.sprite = DamageIntentImage;
                    break;
                case ShieldAllyIntent _:
                    IntentImage.sprite = ShieldIntentImage;
                    break;
                default:
                    IntentImage.gameObject.SetActive(false);
                    break;
            }
        }
    }
}
