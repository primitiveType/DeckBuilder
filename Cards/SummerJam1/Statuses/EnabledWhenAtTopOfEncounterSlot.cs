using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
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
            UpdateEnabledState();
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

            DetachFromParentCollectionChanged();

            CurrentParent = component;
            CurrentParent.Entity.Children.CollectionChanged += ParentChildrenChanged;

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

            //if we are not at the top of the stack.
            if (CurrentParent.Entity.Children.LastOrDefault() != Entity)
            {
                Enabled = false;
                return;
            }

            Enabled = true;
        }

        private void DetachFromParentCollectionChanged()
        {
            if (CurrentParent != null)
            {
                CurrentParent.Entity.Children.CollectionChanged -= ParentChildrenChanged;
            }
        }

        public EncounterSlotPile CurrentParent { get; set; }

        private void ParentChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateEnabledState();
        }


        public override void Terminate()
        {
            base.Terminate();
            Entity.PropertyChanged -= EntityOnPropertyChanged;
            Entity.Components.CollectionChanged -= ComponentsOnCollectionChanged;
            DetachFromParentCollectionChanged();
        }
    }
}
