using System.Collections.Generic;
using Api;
using SummerJam1.Statuses;

namespace SummerJam1
{
    //intents need to be associated with an enemy to show the intent above that enemy.
    //they also need to be associated with a target beat, to show the intent in the beat tracker.
    //we basically need to create two views for the same component.
    //worse, we can have multiple intents, and on the enemy itself we only show the next upcoming intent.
    public abstract class Intent : SummerJam1Component, IVisual
    {
        public int TargetBeat { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
        }

        [OnBeatMoved]
        private void OnBeatMoved(object sender, BeatMovedEventArgs args)
        {
            if (TargetBeat > Game.Battle.BeatTracker.MaxBeatsToThreshold)
            {
                //the threshold dropped below our target, so we are destroyed.
                Entity.Destroy();
            }
            if (args.DidOverload || args.Current >= TargetBeat)
            {
                DoIntent();
            }
        }


        public void DoIntent()
        {
            OnTrigger();
            Entity.Destroy();
        }

        protected abstract void OnTrigger();
    }
}
