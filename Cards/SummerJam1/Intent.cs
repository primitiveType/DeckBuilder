using System.Collections.Generic;
using SummerJam1.Statuses;

namespace SummerJam1
{
    //intents need to be associated with an enemy to show the intent above that enemy.
    //they also need to be associated with a target beat, to show the intent in the beat tracker.
    //we basically need to create two views for the same component.
    //worse, we can have multiple intents, and on the enemy itself we only show the next upcoming intent.
    public abstract class Intent : SummerJam1Component
    {
        public int TargetBeat { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            Game.Battle.BeatTracker.RegisterIntent();
        }

        [OnBeatMoved]
        private void OnBeatMoved(object sender, BeatMovedEventArgs args)
        {
            if (args.DidOverload || args.Current >= TargetBeat)
            {
                DoIntent();
            }
        }


        public void DoIntent()
        {
            OnTrigger();
            Entity.RemoveComponent(this);
        }

        protected abstract void OnTrigger();
    }
}
