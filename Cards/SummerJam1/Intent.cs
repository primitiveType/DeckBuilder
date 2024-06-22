using Api;

namespace SummerJam1
{
    //intents need to be associated with an enemy to show the intent above that enemy.
    public abstract class Intent : SummerJam1Component, IVisual
    {
        [OnIntentStarted]
        private void OnIntentStarted(object sender, IntentStartedEventArgs args)
        {
            DoIntent();
        }


        public void DoIntent()
        {
            OnTrigger();
            Entity.RemoveComponent<Intent>();
        }

        protected abstract void OnTrigger();
    }
}