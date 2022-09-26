using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Api;
using CardsAndPiles;
using SummerJam1.Piles;

namespace SummerJam1.Statuses
{
    public abstract class EnabledWhenAtTopOfEncounterSlot : SummerJam1Component
    {
        protected bool EnableOnTurnStart { get; set; }

        private IEntity CachedParent { get; set; }
        protected override void Initialize()
        {
            base.Initialize();
            Entity.PropertyChanged += EntityOnPropertyChanged;
            HandleParentChanged();//get things set up for the initial state.
        }

        private void EntityOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Entity.Parent) )
            {
                HandleParentChanged();
            }
        }

        private void HandleParentChanged()
        {
            DetachFromOldParent();
            AttachToNewParent();
            UpdateEnabledState();
        }

        private void AttachToNewParent()
        {
            EncounterSlotPile component = Entity.GetComponentInParent<EncounterSlotPile>();
            if (component == null)
            {//don't bother attaching if not in an encounter slot.
                return;
            }
            Entity.Parent.Children.CollectionChanged += OnParentCollectionChanged;
            CachedParent = Entity.Parent;
        }

        private void DetachFromOldParent()
        {
            if (CachedParent == null)
            {
                return;
            }
            
            CachedParent.Children.CollectionChanged -= OnParentCollectionChanged;
        }

        private void OnParentCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateEnabledState();
        }

        private void UpdateEnabledState()
        {
            EncounterSlotPile component = Entity.GetComponentInParent<EncounterSlotPile>();
            if (component == null)
            {
                Enabled = false;
            }

            if (Game.Battle != null && Entity.Parent != null && Entity.Parent.Children.LastOrDefault() == Entity)
            {
                EnableOnTurnStart = true;
            }
            else
            {
                Enabled = false;
            }
        }

        [OnTurnBegan]
        private void OnTurnBegan(object sender, TurnBeganEventArgs args)
        {
            if (EnableOnTurnStart)
            {
                EnableOnTurnStart = false;
                Enabled = true;
            }
        }

        public override void Terminate()
        {
            base.Terminate();
            Entity.PropertyChanged -= EntityOnPropertyChanged;
            DetachFromOldParent();
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
