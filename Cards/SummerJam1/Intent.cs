using System.Collections.Generic;
using CardsAndPiles;

namespace SummerJam1
{
    public abstract class Intent : SummerJam1Component
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

        [OnTurnEnded]
        private void OnTurnEnded()
        {
            DoIntent();
        }

        protected abstract void DoIntent();
    }
}