using Api;
using CardsAndPiles.Components;
using SummerJam1.Piles;
using SummerJam1.Statuses;

namespace SummerJam1.Units.Effects
{
    public abstract class CreaturesMovedOnTopAreGiven<TAmount> : SummerJam1Component, IDescription, IAmount
        where TAmount : Component, IAmount, new()
    {
        public int Amount { get; set; }

        [OnUnitMoved]
        private void OnUnitMoved(object sender, UnitMovedEventArgs args)
        {
            var mySlot = Entity.GetComponentInParent<EncounterSlotPile>();
            if (mySlot == null || mySlot.Entity != args.CardId.Parent)
            {
                return;
            }

            if (mySlot.Entity.Children[mySlot.Entity.Children.Count - 2] != Entity)
            {//exit if second-to-last child is not us.
                return;
            }
            
            if (args.Target.GetComponentInSelfOrParent<EncounterSlotPile>() != mySlot)
            {
                return;
            }

            //target is moved into our slot while we are active.
            args.CardId.GetOrAddComponent<TAmount>().Amount += Amount;
        }

        public string Description => $"Permanently grants {Amount} {GetEffectName()} to units placed directly on top of it.";
        protected abstract string GetEffectName();
    }
}
