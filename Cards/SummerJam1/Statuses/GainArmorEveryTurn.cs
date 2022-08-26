using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using PropertyChanged;

namespace SummerJam1.Statuses
{
    public class GainArmorEveryTurn : EnabledWhenInEncounterSlot, IStatusEffect, ITooltip, IAmount
    {
        public int Amount { get; set; }

        [DependsOn(nameof(Amount))] public string Tooltip => $"Gains {Amount} Armor every turn.";

        [OnTurnBegan]
        private void OnTurnBegan()
        {
            Entity.GetOrAddComponent<Armor>().Amount += Amount;
        }
    }
}
