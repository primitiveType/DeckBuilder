using Api;
using CardsAndPiles.Components;

namespace SummerJam1.Cards.Effects
{
    public class GivePlayerStrength : SummerJam1Component, IAmount, IEffect, IDescription, ITooltip
    {
        public int Amount { get; set; }

        public string Description => $"Gain {Amount} Strength.";

        public bool DoEffect(IEntity target)
        {
            Game.Player.Entity.GetOrAddComponent<Strength>().Amount += Amount;
            return true;
        }

        public string Tooltip => StrengthTooltip.s_Tooltip;
    }
}