using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Units.Effects;

namespace SummerJam1
{
    public class DamageIntent : Intent, IAmount
    {
        public int Amount { get; set; }

        public int Attacks { get; set; } = 1;

       
        protected override void OnTrigger()
        {
            if (!Enabled)
            {
                return;
            }

            IEntity targetSlot = Game.Player.Entity;

            if (targetSlot == null)
            {
                return;
            }

            Events.OnIntentStarted(new IntentStartedEventArgs(Entity));


            foreach (ITakesDamage componentsInChild in targetSlot.GetComponentsInChildren<ITakesDamage>())
            {
                for (int i = 0; i < Attacks; i++)
                {
                    componentsInChild.TryDealDamage(Amount, Entity);
                }
            }
        }

        public int GetEffectiveDamage(IEntity targetEntity)
        {
            if (State == LifecycleState.Destroyed)
            {
                return 0;
            }
            ITakesDamage component = targetEntity.GetComponentInChildren<ITakesDamage>();
            return component.GetEffectiveDamage(Amount, Entity);
        }
    }
}
