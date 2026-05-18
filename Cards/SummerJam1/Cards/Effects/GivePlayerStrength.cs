using Api;
using CardsAndPiles.Components;

namespace SummerJam1.Cards.Effects
{
    public class GivePlayerStrength : SummerJam1Component, IAmount, IEffect, IDescription, ITooltip
    {
        public TargetingType Targeting{ get; } = TargetingType.Player;

        public int Amount { get; set; }

        public string Description => $"Gain {Amount} Strength.";

        public bool DoEffect(IEntity target)
        {
            (target ?? Game.Player.Entity).GetOrAddComponent<Strength>().Amount += Amount;
            return true;
        }

        public string Tooltip => StrengthTooltip.STRENGTH_TOOLTIP;
    }
}
