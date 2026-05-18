using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1.Cards.Effects
{
    public class GrantArmorToPlayer : SummerJam1Component, IEffect, IDescription, ITooltip
    {
        public TargetingType Targeting{ get; } = TargetingType.Player;

        [JsonProperty] public int BlockAmount { get; private set; }

        [JsonIgnore] public string Description => $"Gain {BlockAmount} block.";


        public bool DoEffect(IEntity target)
        {
            Armor armor = (target ?? Game.Player.Entity).GetOrAddComponent<Armor>();
            armor.Amount += BlockAmount;
            return true;
        }

        [JsonIgnore] public string Tooltip => Tooltips.ARMOR_TOOLTIP;
    }
}
