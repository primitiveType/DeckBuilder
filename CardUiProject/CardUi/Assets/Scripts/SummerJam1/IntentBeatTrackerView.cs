using App;
using TMPro;
using UnityEngine;

namespace SummerJam1
{
    public class IntentBeatTrackerView : View<Intent>
    {
        private BeatTrackerView Tracker { get; set; }

        public void SetBeatTracker(BeatTrackerView beatTrackerView)
        {
            Tracker = beatTrackerView;
        }


        [PropertyListener(nameof(Intent.TargetBeat))]
        protected void TargetBeatChanged()
        {
            transform.SetParent(Tracker.GetTickTransform(Model.TargetBeat));
        }
        
    }
}
