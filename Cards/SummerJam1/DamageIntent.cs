using System.ComponentModel;
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

        public bool IgnoreStealth { get; set; }

        private Player Player { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            Player = Context.Root.GetComponent<Game>().Player;
            Player.PropertyChanged += PlayerOnPropertyChanged;
        }

        public override void Terminate()
        {
            base.Terminate();
            Player.PropertyChanged -= PlayerOnPropertyChanged;
        }

        private void PlayerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!IgnoreStealth)
            {
                Enabled = Player.CurrentStealth <= 0;
            }
        }

        protected override void DoIntent()
        {
            if (!Enabled)
            {
                return;
            }

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
                        var player = Context.Root.GetComponent<Game>().Player;
                        if (IgnoreStealth || player.CurrentStealth <= 0)
                        {
                            targetSlot = player.Entity;
                        }
                    }
                }

                return targetSlot;
            }
        }
    }
}
