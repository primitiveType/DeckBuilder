using Api;
using CardsAndPiles.Components;
using Newtonsoft.Json;

namespace SummerJam1.Cards.Effects
{
    public class GrantArmorToPlayer : SummerJam1Component, IEffect, IDescription, ITooltip
    {
        [JsonProperty] public int BlockAmount { get; private set; }

        [JsonIgnore] public string Description => $"Gain {BlockAmount} block.";


        public bool DoEffect(IEntity target)
        {
            Armor armor = Game.Player.Entity.GetOrAddComponent<Armor>();
            armor.Amount += BlockAmount;
            return true;
        }

        [JsonIgnore] public string Tooltip => Tooltips.ARMOR_TOOLTIP;
    }
    public class GrantStrengthToPlayer : SummerJam1Component, IEffect, IDescription, ITooltip, IAmount
    {
        [JsonProperty] public int Amount { get; set; }

        [JsonIgnore] public string Description => $"Gain {Amount} strength.";


        public bool DoEffect(IEntity target)
        {
            Armor armor = Game.Player.Entity.GetOrAddComponent<Armor>();
            armor.Amount += Amount;
            return true;
        }

        [JsonIgnore] public string Tooltip => Tooltips.ARMOR_TOOLTIP;
    }
}
