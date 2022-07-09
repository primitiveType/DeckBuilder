using System.Linq;
using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Units;

namespace SummerJam1
{
    public class DamageIntent : Intent
    {
        [JsonIgnore] public int Amount => Entity.GetComponent<Strength>()?.Amount ?? 0;
        [JsonIgnore] private int Attacks => 1 + (Entity.GetComponent<MultiAttack>()?.Amount ?? 0);

        protected override void DoIntent()
        {
            bool isFriendly = Entity.GetComponentInParent<FriendlyUnitSlot>() != null;

            IEntity targetSlot;
            targetSlot = GetTargetSlot();

            if (targetSlot == null)
            {
                return;
            }

            Events.OnIntentStarted(new IntentStartedEventArgs(Entity));

            for (int i = 0; i < Attacks; i++)
            {
                foreach (ITakesDamage componentsInChild in targetSlot.GetComponentsInChildren<ITakesDamage>())
                {
                    componentsInChild.TryDealDamage(Amount, Entity);
                    targetSlot = GetTargetSlot();
                }
            }

            IEntity GetTargetSlot()
            {
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
                        targetSlot = Context.Root.GetComponent<Game>().Player.Entity;
                    }
                }

                return targetSlot;
            }
        }
    }
}
