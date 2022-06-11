using System.Linq;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Units;

namespace SummerJam1
{
    public class DamageIntent : Intent, IAmount
    {
        [JsonIgnore] private int Amount => Entity.GetComponent<Strength>()?.Amount ?? 0;

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
}