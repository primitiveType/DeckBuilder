using System.Linq;
using SummerJam1.Cards;

namespace SummerJam1.Rules
{
    public class MovedUnitMustHaveLessHealthThanUnitUnderneath : SummerJam1Component
    {
        [OnRequestMoveUnit]
        private void OnRequestMoveUnit(object sender, RequestMoveUnitEventArgs args)
        {
            Health topMonsterHealth = args.Target.Children.LastOrDefault()?.GetComponent<Health>();
            Health myHealth = args.CardId.GetComponent<Health>();
            if (topMonsterHealth != null && topMonsterHealth.Amount <= myHealth.Amount)
            {
                args.Blockers.Add(CardBlockers.TOP_MONSTER_HAS_LESS_HEALTH);
            }
        }
    }
}
