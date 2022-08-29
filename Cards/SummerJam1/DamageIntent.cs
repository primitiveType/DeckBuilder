using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1
{
    public class DamageIntent : Intent
    {
        [JsonIgnore] public int Amount => 0;
        [JsonIgnore] private int Attacks => 1 + (Entity.GetComponent<MultiAttack>()?.Amount ?? 0);


        private Player Player { get; set; }


        protected override void Initialize()
        {
            base.Initialize();
            Player = Context.Root.GetComponent<Game>().Player;
        }


        public override void DoIntent()
        {
            if (!Enabled)
            {
                return;
            }

            bool isFriendly = false;

            IEntity targetSlot;
            targetSlot = Game.Player.Entity;

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
                }
            }
        }
    }
}
