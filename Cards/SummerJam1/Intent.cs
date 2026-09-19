using Api;

namespace SummerJam1
{
    //intents need to be associated with an enemy to show the intent above that enemy.
    public abstract class Intent : SummerJam1Component
    {

        [OnAttackPhaseStarted]
        private void OnAttackPhaseStarted(object sender, AttackPhaseStartedEventArgs args)
        {
            DoIntent();
        }


        public void DoIntent()
        {
            OnTrigger();
        }

        protected abstract void OnTrigger();
    }
}