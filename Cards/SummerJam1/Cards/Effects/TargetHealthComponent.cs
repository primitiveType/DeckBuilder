using CardsAndPiles;

namespace SummerJam1.Cards.Effects
{
    public class TargetHealthComponent : SummerJam1Component
    {
        [OnRequestPlayCard]
        private void OnRequestPlayCard(object sender, RequestPlayCardEventArgs args)
        {
            if (args.CardId != Entity)
            {
                return;
            }

            Health target = args.Target.GetComponentInChildren<Health>();

            if (target == null)
            {
                args.Blockers.Add(CardBlockers.INVALID_TARGET);
            }
        }
    }
}
