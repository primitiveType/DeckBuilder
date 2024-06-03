using CardsAndPiles.Components;

namespace SummerJam1.Relics
{
    public class ReachingXHealthHealsForY : SummerJam1Component, ITooltip, IDescription
    {
        public int TargetHealth { get; set; }
        public int HealAmount { get; set; }
        public string Description => Tooltip;
        public string Tooltip => $"When your health reaches exactly {TargetHealth}, heal for {HealAmount}.";
    }
}