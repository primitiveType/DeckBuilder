using System.Collections.Specialized;
using System.ComponentModel;
using SummerJam1.Cards;
using SummerJam1.Piles;

namespace SummerJam1.Statuses
{
    public abstract class EnabledWhenAtTopOfEncounterSlot : SummerJam1Component
    {
        protected override void Initialize()
        {
            base.Initialize();
            Entity.PropertyChanged += EntityOnPropertyChanged;
            Entity.Components.CollectionChanged += ComponentsOnCollectionChanged;
        }

        private void ComponentsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateEnabledState();
        }

        private void EntityOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Entity.Parent))
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
                return;
            }

            if (Entity.GetComponent<IDisableAbilities>() != null)
            {
                Enabled = false;
                return;
            }

            //if battle has not started, or our parent is null disable us.
            if (Game.Battle == null || Entity.Parent == null)
            {
                Enabled = false;
                return;
            }

            Enabled = true;
        }


        public override void Terminate()
        {
            base.Terminate();
            Entity.PropertyChanged -= EntityOnPropertyChanged;
            Entity.Components.CollectionChanged -= ComponentsOnCollectionChanged;
        }
    }

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