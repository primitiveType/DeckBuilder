using Api;
using CardsAndPiles.Components;
using SummerJam1.Cards;
using SummerJam1.Cards.Effects;

namespace SummerJam1.Statuses
{
    public class GrantThornsToPlayer: SummerJam1Component, IEffect, IDescription, ITooltip, IAmount
    {
        public TargetingType Targeting{ get; } = TargetingType.Player;

        public string Tooltip => Tooltips.THORNS_TOOLTIP;
        public bool DoEffect(IEntity target)
        {
            Thorns armor = Game.Player.Entity.GetOrAddComponent<Thorns>();
            armor.Amount += Amount;
            return true;
        }

        public string Description => $"Gain {Amount} Thorns";
        public int Amount { get; set; } = 1;
    }
}