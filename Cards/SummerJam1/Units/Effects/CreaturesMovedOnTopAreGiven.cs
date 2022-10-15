using Api;
using CardsAndPiles.Components;
using SummerJam1.Piles;
using SummerJam1.Statuses;

namespace SummerJam1.Units.Effects
{
    public abstract class CreaturesMovedOnTopAreGiven<TAmount> : EnabledWhenAtTopOfEncounterSlot, IDescription, IAmount
        where TAmount : Component, IAmount, new()
    {
        public int Amount { get; set; }

        [OnUnitMoved]
        private void OnUnitMoved(object sender, UnitMovedEventArgs args)
        {
            //target is moved into our slot while we are active.
            if (args.Target.GetComponentInParent<EncounterSlotPile>() == Entity.GetComponentInParent<EncounterSlotPile>())
            {
                args.CardId.GetOrAddComponent<TAmount>().Amount += Amount;
            }
        }

        public string Description => $"Permanently grants {Amount} {GetEffectName()} to units placed directly on top of it.";
        protected abstract string GetEffectName();
    }
}
