using CardsAndPiles;
using CardsAndPiles.Components;

namespace SummerJam1
{
    public class Lifesteal : SummerJam1Component, ITooltip, IDescription
    {
        public string Description => Tooltip;

        public string Tooltip => "Lifesteal - when this deals damage, it heals for the same amount.";

        [OnDamageDealt]
        private void OnDamageDealt(object sender, DamageDealtEventArgs args)
        {
            if (args.SourceEntityId == Entity)
            {
                Entity.GetComponent<Health>().TryHeal(args.Amount, Entity);
            }
        }
    }
}