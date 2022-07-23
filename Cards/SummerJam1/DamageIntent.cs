using System.ComponentModel;
using Newtonsoft.Json;

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
            Player.Entity.GetComponent<Stealth>().PropertyChanged += PlayerOnPropertyChanged;
        }

        public override void Terminate()
        {
            base.Terminate();
            Player.Entity.GetComponent<Stealth>().PropertyChanged -= PlayerOnPropertyChanged;
        }

        private void PlayerOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!IgnoreStealth)
            {
                Enabled = Player.Entity.GetComponent<Stealth>().Amount <= 0;
            }
        }

        protected override void DoIntent()
        {
            if (!Enabled)
            {
                return;
            }

            //TODO: make intents work.

            // return targetSlot;
        }
    }
}

