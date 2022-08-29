using System.Collections.Generic;
using SummerJam1.Statuses;

namespace SummerJam1
{
    public abstract class Intent : EnabledWhenInEncounterSlot
    {
        protected override void Initialize()
        {
            base.Initialize();
            List<Intent> intents = Entity.GetComponents<Intent>();
            foreach (Intent intent in intents)
            {
                if (intent != this)
                {
                    Entity.RemoveComponent(intent);
                }
            }
        }

        // [OnAttackPhaseStarted]
        // private void OnAttackPhaseStarted()
        // {
        //     DoIntent();
        // }

        public abstract void DoIntent();
    }
}
