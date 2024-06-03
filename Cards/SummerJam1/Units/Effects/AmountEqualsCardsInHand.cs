using System.ComponentModel;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Statuses;
using Component = Api.Component;

namespace SummerJam1.Units.Effects
{
    public abstract class AmountEqualsCardsInHand<T> : EnabledWhenAtTopOfEncounterSlot, ITooltip, IDescription where T : Component, IAmount, new()
    {
        protected abstract string StatName { get; }
        public string Description => $"{StatName} equals the number of cards in your hand.";
        public string Tooltip => Description;

        protected override void Initialize()
        {
            base.Initialize();
            PropertyChanged += ComponentPropertyChanged;
        }

        private void ComponentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Enabled))
            {
                if (Enabled)
                {
                    UpdateStrength();
                }
            }
        }

        [OnCardDrawn]
        private void OnCardDrawn(object sender, CardDrawnEventArgs args)
        {
            UpdateStrength();
        }

        private void UpdateStrength()
        {
            Entity.GetOrAddComponent<T>().Amount = Game.Battle.Hand.Entity.Children.Count;
        }

        [OnCardDiscarded]
        private void OnCardDiscarded(object sender, CardDiscardedEventArgs args)
        {
            UpdateStrength();
        }
    }
}
