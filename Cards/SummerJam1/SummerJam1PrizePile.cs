using System.Linq;
using Api;
using CardsAndPiles;

namespace SummerJam1
{
    public class SummerJam1PrizePile : PrizePile
    {
        [OnBattleEnded]
        private void OnBattleEnded(object sender, BattleEndedEventArgs args)
        {
            if (args.Victory)
            {
                //Player.Entity.TrySetParent(TempPlayerSlot.Entity);
                SetupPrizePile();
            }
        }

        public void SetupPrizePile()
        {
            Clear();
            for (int i = 0; i < 3; i++)
            {
                Entity.GetComponentInParent<SummerJam1Game>().CreateRandomCard().TrySetParent(Entity);
            }
        }

        public void ChoosePrize(IEntity child)
        {
            if (child != null)
            {
                child.TrySetParent(Entity.GetComponentInParent<SummerJam1Game>().Deck.Entity);
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