using System.Collections.Generic;
using Newtonsoft.Json;

namespace SummerJam1
{
    public abstract class Intent : SummerJam1Component
    {
        [JsonIgnore] public virtual bool Enabled { get; set; } = true;

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

        [OnAttackPhaseStarted]
        private void OnAttackPhaseStarted()
        {
            DoIntent();
        }

        protected abstract void DoIntent();
    }
}
