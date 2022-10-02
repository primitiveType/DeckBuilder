using System.ComponentModel;
using SummerJam1.Piles;

namespace SummerJam1.Statuses
{
    public abstract class EnabledWhenInEncounterSlot : SummerJam1Component
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.PropertyChanged += EntityOnPropertyChanged;

            if (Game.Battle != null)
            {
                Game.Battle.PropertyChanged += EntityOnPropertyChanged;
            }

            UpdateEnabledState();
        }

        private void EntityOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Entity.Parent) || e.PropertyName == nameof(BattleContainer.CurrentFloor))
            {
                UpdateEnabledState();
            }
        }

        private void UpdateEnabledState()
        {
            EncounterSlotPile component = Entity.GetComponentInParent<EncounterSlotPile>();
            if (component == null)
            {
                Enabled = false;
            }

            Enabled = Game.Battle != null && Game.Battle.EncounterSlots.Contains(component);
        }

        public override void Terminate()
        {
            base.Terminate();
            Entity.PropertyChanged -= EntityOnPropertyChanged;
        }
    }
}