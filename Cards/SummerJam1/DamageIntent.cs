using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Units.Effects;

namespace SummerJam1
{
    public class DamageIntent : Intent, IAmount
    {
        [JsonIgnore] public int Amount { get; set; }

        [JsonIgnore] private int Attacks => 1 + (Entity.GetComponent<MultiAttack>()?.Amount ?? 0);

       
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
                    componentsInChild.TryDealDamage(Amount, Entity.Parent);
                }
            }
        }

        public int GetEffectiveDamage(IEntity targetEntity)
        {
            ITakesDamage component = targetEntity.GetComponentInChildren<ITakesDamage>();
            return component.GetEffectiveDamage(Amount, Entity.Parent);
        }
    }
}
