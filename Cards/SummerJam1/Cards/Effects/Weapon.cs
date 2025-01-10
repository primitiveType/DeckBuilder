using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Statuses;

namespace SummerJam1.Cards.Effects
{
    public class Weapon : SummerJam1Component, ITooltip
    {
        public string Tooltip => "Becomes bloodied when dealing damage.";

        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
        
        }

        [OnDamageDealt]
        private void OnDamageDealt(object sender, DamageDealtEventArgs args)
        {
            if (args.SourceEntityId == Entity)
            {
                if (!Entity.HasComponent<Bloodied>())
                {
                    Entity.AddComponent<Bloodied>();
                }
                Events.OnCardGainedStatus(new CardGainedStatusEventArgs(Entity, nameof(Bloodied)));
            }
        }
    }
}