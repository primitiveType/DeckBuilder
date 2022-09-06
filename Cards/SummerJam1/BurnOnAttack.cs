using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Statuses;

namespace SummerJam1
{
    public class BurnOnAttack : EnabledWhenInEncounterSlot, ITooltip, IDescription, IAmount
    {
        public int Amount { get; set; } = 1;
        public string Description => Tooltip;


        public string Tooltip => $"Fire Breath - when attacking, adds {Amount} burn.";

        [OnDamageDealt]
        private void OnDamageDealt(object sender, DamageDealtEventArgs args)
        {
            if (args.SourceEntityId == Entity)
            {
                args.EntityId.GetOrAddComponent<Burn>().Amount += Amount;
            }
        }
    }
}