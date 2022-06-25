using System.Linq;
using Api;
using CardsAndPiles;
using SummerJam1.Objectives;

namespace SummerJam1
{
    public class SummerJam1RelicPrizePile : PrizePile
    {
        [OnLeaveBattle]
        private void OnBattleEnded()
        {
            //Player.Entity.TrySetParent(TempPlayerSlot.Entity);
            var game = Context.Root.GetComponentInChildren<SummerJam1Game>();
            var objective = game.Battle.ObjectivesPile.Entity.GetComponentInChildren<Objective>();
            if (objective.Completed && !objective.Failed)
            {
                SetupPrizePile();
            }
        }

        public void SetupPrizePile()
        {
            Clear();
            for (int i = 0; i < 1; i++)
            {
                Entity.GetComponentInParent<SummerJam1Game>().CreateRandomRelic().TrySetParent(Entity);
            }
        }

        public void ChoosePrize(IEntity child)
        {
            if (child != null)
            {
                child.TrySetParent(Entity.GetComponentInParent<SummerJam1Game>().RelicPile.Entity);
            }

            Clear();
        }

        private void Clear()
        {
            foreach (IEntity unwantedChild in Entity.Children.ToList())
            {
                unwantedChild.Destroy();
            }
        }
    }
}