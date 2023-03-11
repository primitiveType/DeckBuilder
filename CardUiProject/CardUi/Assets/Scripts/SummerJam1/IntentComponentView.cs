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

        private BeatTrackerView BeatTrackerView { get; set; }
        protected override void Start()
        {
            base.Start();
            // var btGo = Entity.Context.Root.GetComponent<Game>().Battle.BeatTracker.Entity.GetComponent<IGameObject>().gameObject;
            // Debug.Log($"Beat tracker go = {btGo}.");
            // BeatTrackerView = btGo.GetComponent<BeatTrackerView>();//the beattracker view, hopefully?
        }

        protected override void ComponentOnPropertyChanged()
        {
            UpdateIntentImage();
            Debug.Log($"Intent changed. Beat tracker {BeatTrackerView?.name}.");
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
