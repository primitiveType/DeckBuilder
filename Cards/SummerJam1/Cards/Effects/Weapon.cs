using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Statuses;

namespace SummerJam1.Cards.Effects
{
    public class Weapon : SummerJam1Component, ITooltip
    {
        public string Tooltip { get; } = "Becomes bloodied when Played.";

        [OnCardPlayed]
        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            if (args.CardId == Entity)
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