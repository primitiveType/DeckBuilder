using CardsAndPiles;
using SummerJam1.Units;

namespace SummerJam1.Statuses
{
    public class Bloodied : SummerJam1Component
    {
        protected override void Initialize()
        {
            base.Initialize();
            Events.OnCardGainedStatus(new CardGainedStatusEventArgs(Entity, nameof(Bloodied)));
        }
    }
}