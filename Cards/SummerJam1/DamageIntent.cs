using System.Linq;
using Api;
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
            bool isFriendly = Entity.GetComponentInParent<FriendlyUnitSlot>() != null;


            IEntity targetSlot;
            if (isFriendly)
            {
                targetSlot = Context.Root.GetComponentsInChildren<EnemyUnitSlot>()
                    .FirstOrDefault(slot => slot.Entity.GetComponentInChildren<Unit>() != null)?.Entity;
            }
            else
            {
                targetSlot = Context.Root.GetComponentsInChildren<FriendlyUnitSlot>()
                    .FirstOrDefault(slot => slot.Entity.GetComponentInChildren<Unit>() != null)?.Entity;

                if (targetSlot == null)
                {
                    targetSlot = Context.Root.GetComponent<SummerJam1Game>().Player.Entity;
                }
            }

            if (targetSlot == null)
            {
                return;
            }

            Events.OnIntentStarted(new IntentStartedEventArgs(Entity));

            foreach (ITakesDamage componentsInChild in targetSlot.GetComponentsInChildren<ITakesDamage>())
            {
                componentsInChild.TryDealDamage(Amount, Entity);
            }
        }
    }
}