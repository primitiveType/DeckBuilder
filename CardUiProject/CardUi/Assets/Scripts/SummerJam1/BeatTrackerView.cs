using System.Collections.Generic;
using System.ComponentModel;
using Api;
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
        [SerializeField] private IntentBeatTrackerView IntentPrefab;

        private Dictionary<int, GameObject> TickTransforms { get; } = new();

        protected void Awake()
        {
            if (!Debug)
            {
                Game game = GameContext.Instance.Context.Root.GetComponent<Game>();
                Disposables.Add(GameContext.Instance.Context.Events.SubscribeToEntityCreated(OnEntityCreated));


                BeatTracker beatTracker = game.Battle.BeatTracker;
                SetModel(beatTracker.Entity);
                BeatTracker = beatTracker;
            }
            else
            {
                MockBeatTracker beatTracker = new()
                {
                    MaxBeatsToThreshold = 10
                };
                BeatTracker = beatTracker;
                UpdateDebugBeatTracker();
            }

            BeatTracker.PropertyChanged += BeatTrackerOnPropertyChanged;
            UpdateSliderArea();
            UpdateNotches();
            if (!Debug)
            {
                foreach (Intent intent in GameContext.Instance.Context.Root.GetComponentsInChildren<Intent>())
                { //it's smelly in here.
                    CreateViewForIntent(intent);
                }
            }
        }

        protected override void Start()
        {
            if (!Debug)
            {
                base.Start();
            }
        }

        private void Update()
        {
            if (Debug)
            {
                UpdateDebugBeatTracker();
            }

            UpdateSliderValue();
        }

        public IBeatTracker BeatTracker { get; private set; }

        private void OnEntityCreated(object sender, EntityCreatedEventArgs item)
        {
            Intent intent = item.Entity.GetComponent<Intent>();
            if (intent == null)
            {
                return;
            }

            CreateViewForIntent(intent);
        }

        private void CreateViewForIntent(Intent intent)
        {
            IntentBeatTrackerView go = Instantiate(IntentPrefab);
            go.SetBeatTracker(this);
            go.SetModel(intent.Entity);
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

            TickTransforms.Clear();
            for (int i = 0; i < BeatTracker.MaxBeatsToThreshold; i++)
            {
                TickTransforms.Add(i, Instantiate(NotchPrefab, NotchParent));
            }
        }

        private void UpdateSliderArea()
        {
            float halfOneStep = .5f / BeatTracker.MaxBeatsToThreshold;

            m_CurrentBeatSlideArea.anchoredPosition = new Vector2();
            m_CurrentBeatSlideArea.sizeDelta = new Vector2(1, 1);
            m_CurrentBeatSlideArea.anchorMin = new Vector2(halfOneStep, 0);
            m_CurrentBeatSlideArea.anchorMax = new Vector2(1 - halfOneStep, 1);
            m_CurrentBeatSlideArea.offsetMin = new Vector2(0, 0);
            m_CurrentBeatSlideArea.offsetMax = new Vector2(1, 1);
            m_CurrentBeatSlideArea.anchoredPosition = new Vector2();
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

        public Transform GetTickTransform(int modelTargetBeat)
        {
            return TickTransforms[modelTargetBeat].transform;
        }
    }

    public interface IBeatTrackerProvider
    {
        public IBeatTracker BeatTracker { get; }
    }
}
