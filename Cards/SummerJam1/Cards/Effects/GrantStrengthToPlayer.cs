using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1.Cards.Effects
{
    public class GrantStrengthToPlayer : SummerJam1Component, IEffect, IDescription, ITooltip, IAmount
    {
        public TargetingType Targeting{ get; } = TargetingType.Self;

        [JsonProperty] public int Amount { get; set; }

        public string Description => $"Gain {Amount} strength.";


        public bool DoEffect(IEntity target)
        {
            Strength strength = (target ?? Game.Player.Entity).GetOrAddComponent<Strength>();
            strength.Amount += Amount;
            return true;
        }

        public string Tooltip => Tooltips.STRENGTH_TOOLTIP;
    }
}
