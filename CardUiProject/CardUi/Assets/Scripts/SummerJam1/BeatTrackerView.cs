using System.ComponentModel;
using App;
using App.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class BeatTrackerView : View<BeatTracker>, IBeatTrackerProvider
    {
        public bool Debug;

        public int DebugCurrentBeat;
        public int DebugMaxBeats = 10;

        [SerializeField] private Slider m_CurrentBeatSlider;
        [SerializeField] private RectTransform m_CurrentBeatSlideArea;
        [SerializeField] private Transform NotchParent;
        [SerializeField] private GameObject NotchPrefab;

        protected void Awake()
        {
            if (!Debug)
            {
                Game game = GameContext.Instance.Context.Root.GetComponent<Game>();
                BeatTracker beatTracker = game.Battle.BeatTracker;
                SetModel(beatTracker.Entity);
                BeatTracker = beatTracker;
            }
            else
            {
                var beatTracker = new MockBeatTracker();
                beatTracker.MaxBeatsToThreshold = 10;
                BeatTracker = beatTracker;
                UpdateDebugBeatTracker();
            }

            BeatTracker.PropertyChanged += BeatTrackerOnPropertyChanged;
            UpdateSliderArea();
            UpdateNotches();
        }

        protected override void Start()
        {
            if (!Debug)
            {
                base.Start();
            }
        }

        private void BeatTrackerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SummerJam1.BeatTracker.MaxBeatsToThreshold))
            {
                UpdateSliderArea();
                UpdateNotches();
            }
        }

        private void UpdateNotches()
        {
            foreach (Transform child in NotchParent)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < BeatTracker.MaxBeatsToThreshold; i++)
            {
                Instantiate(NotchPrefab, NotchParent);
            }
        }

        private void UpdateSliderArea()
        {
            float halfOneStep = .5f / (BeatTracker.MaxBeatsToThreshold);

            m_CurrentBeatSlideArea.anchoredPosition = new Vector2();
            m_CurrentBeatSlideArea.sizeDelta = new Vector2(1, 1);
            m_CurrentBeatSlideArea.anchorMin = new Vector2(halfOneStep, 0);
            m_CurrentBeatSlideArea.anchorMax = new Vector2(1 - halfOneStep, 1);
            m_CurrentBeatSlideArea.offsetMin = new Vector2(0, 0);
            m_CurrentBeatSlideArea.offsetMax = new Vector2(1, 1);
            m_CurrentBeatSlideArea.anchoredPosition = new Vector2();
        }

        private void Update()
        {
            if (Debug)
            {
                UpdateDebugBeatTracker();
            }

            UpdateSliderValue();
        }

        private void UpdateSliderValue()
        {
            m_CurrentBeatSlider.value = VectorExtensions.Damp(m_CurrentBeatSlider.value, BeatTracker.CurrentBeat, 5, Time.deltaTime);
            m_CurrentBeatSlider.maxValue = BeatTracker.MaxBeatsToThreshold - 1;
        }

        private void UpdateDebugBeatTracker()
        {
            ((MockBeatTracker)BeatTracker).CurrentBeat = DebugCurrentBeat;
            ((MockBeatTracker)BeatTracker).MaxBeatsToThreshold = DebugMaxBeats;
           
        }

        public IBeatTracker BeatTracker { get; private set; }
    }

    public interface IBeatTrackerProvider
    {
        public IBeatTracker BeatTracker { get; }
    }
}
