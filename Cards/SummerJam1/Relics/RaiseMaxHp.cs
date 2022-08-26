using System.ComponentModel;
using CardsAndPiles.Components;

namespace SummerJam1.Relics
{
    public class RaiseMaxHp : SummerJam1Component, IDescription
    {
        public int Amount { get; set; }

        public string Description => $"On pickup, raise your max HP by {Amount}";

        protected override void Initialize()
        {
            base.Initialize();
            Entity.PropertyChanged += EntityOnPropertyChanged;
        }

        private void EntityOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Entity.Parent))
            {
                if (Entity.Parent == Game.RelicPile.Entity)
                {
                    Health health = Game.Player.Entity.GetComponent<Health>();
                    health.SetMax(health.Max + Amount);
                }
            }
        }

        public override void Terminate()
        {
            base.Terminate();
            Entity.PropertyChanged -= EntityOnPropertyChanged;
        }
    }
}
