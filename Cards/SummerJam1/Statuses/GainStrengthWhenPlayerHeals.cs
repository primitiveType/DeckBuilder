using Api;
using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1.Statuses
{
    public class GainStrengthWhenPlayerHeals : EnabledWhenAtTopOfEncounterSlot, IAmount, ITooltip
    {
        public int Amount { get; set; }
        public string Tooltip => $"Tough - Whenever you heal, gains {Amount} Strength";

        [OnHealDealt]
        private void OnHealDealt(object sender, HealDealtEventArgs args)
        {
            if (args.EntityId == Game.Player.Entity)
            {
                Entity.GetOrAddComponent<Strength>().Amount += Amount;
            }
        }
    }
}
