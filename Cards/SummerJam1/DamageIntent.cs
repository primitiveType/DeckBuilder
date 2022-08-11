using System.ComponentModel;
using System.Linq;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using Newtonsoft.Json;
using SummerJam1.Units;

namespace SummerJam1
{
    public class DamageIntent : Intent
    {
        private bool _enabled;
        [JsonIgnore] public int Amount => Entity.GetComponent<Strength>()?.Amount ?? 0;
        [JsonIgnore] private int Attacks => 1 + (Entity.GetComponent<MultiAttack>()?.Amount ?? 0);

        public bool IgnoreStealth { get; set; }

        private Player Player { get; set; }

        public override bool Enabled
        {
            get => Player.Entity.GetComponent<Stealth>().Amount <= 0 && Entity.GetComponentInParent<EncounterSlotPile>() != null;
            set => _enabled = value;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Player = Context.Root.GetComponent<Game>().Player;
        }


        protected override void DoIntent()
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
