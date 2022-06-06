using System.Collections.Generic;
using System.Linq;
using CardsAndPiles.Components;
using SummerJam1.Units;

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
            Entity.RemoveComponent(this);
        }

        protected abstract void DoIntent();
    }

    public class DamageIntent : Intent, IAmount
    {
        public int Amount { get; set; }

        protected override void DoIntent()
        {
            var isFriendly = Entity.GetComponentInParent<FriendlyUnitSlot>() != null;


            UnitSlot targetSlot;
            if (isFriendly)
            {
                targetSlot = Context.Root.GetComponentsInChildren<EnemyUnitSlot>()
                    .FirstOrDefault(slot => slot.Entity.GetComponentInChildren<Unit>() != null);
            }
            else
            {
                targetSlot = Context.Root.GetComponentsInChildren<FriendlyUnitSlot>()
                    .FirstOrDefault(slot => slot.Entity.GetComponentInChildren<Unit>() != null);
            }

            if (targetSlot == null)
            {
                return;
            }

            Events.OnIntentStarted(new IntentStartedEventArgs(Entity));

            foreach (ITakesDamage componentsInChild in targetSlot.Entity.GetComponentsInChildren<ITakesDamage>())
            {
                componentsInChild.TryDealDamage(Amount, Entity);
            }
        }
    }

    public interface IAmount
    {
        int Amount { get; }
    }
}