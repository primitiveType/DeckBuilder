using Api;
using CardsAndPiles.Components;
using SummerJam1.Piles;

namespace SummerJam1.Units.Effects
{
    public abstract class AfterMoveCardBelowIsGiven<TAmount> : SummerJam1Component, IDescription, IAmount
        where TAmount : Component, IAmount, new()
    {
        public int Amount { get; set; }

        [OnUnitMoved]
        private void OnUnitMoved(object sender, UnitMovedEventArgs args)
        {
            if (args.CardId != Entity)
            {
                return;
            }
            
            EncounterSlotPile mySlot = Entity.GetComponentInParent<EncounterSlotPile>();
            if (mySlot == null)
            {//we were moved outside of slots.
                return;
            }

            if (mySlot.Entity.Children.Count <= 1)
            {//we are the only card in the pile.
                return;
            }

            if (mySlot.Entity.Children[mySlot.Entity.Children.Count - 1] != Entity)
            {//exit if last child is not us.
                return;
            }

            IEntity cardBelow = mySlot.Entity.Children[mySlot.Entity.Children.Count - 2];
            cardBelow.GetOrAddComponent<TAmount>().Amount += Amount;
        }

        public string Description => $"Permanently grants {Amount} {GetEffectName()} to units it is placed onto.";
        protected abstract string GetEffectName();
    }
}
