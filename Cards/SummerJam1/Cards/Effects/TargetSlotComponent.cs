using CardsAndPiles;

namespace SummerJam1.Cards.Effects
{
    public class TargetSlotComponent : SummerJam1Component
    {
        [OnRequestPlayCard]
        private void OnRequestPlayCard(object sender, RequestPlayCardEventArgs args)
        {
            if (args.CardId != Entity)
            {
                return;
            }

            EncounterSlotPile target = args.Target.GetComponentInChildren<EncounterSlotPile>();

            if (target == null || !Game.Battle.EncounterSlots.Contains(target))
            {
                args.Blockers.Add(CardBlockers.INVALID_TARGET);
            }
        }
    }
}
