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
                // SetupPrizePile();
            }
        }

        public void SetupRandomPrizePile()
        {
            Clear();
            for (int i = 0; i < 3; i++)
            {
                Entity.GetComponentInParent<Game>().CreateRandomCard().TrySetParent(Entity);
            }
        }

        public void AddPrefab(string prefab)
        {
            Context.CreateEntity(Entity, prefab);
        }

        public void ChooseAllPrizes()
        {
            foreach (IEntity child in Entity.Children.ToList())
            {
                child.TrySetParent(Entity.GetComponentInParent<Game>().Deck.Entity);
            }

            Clear();
        }

        public void ChoosePrize(IEntity child)
        {
            if (child != null)
            {
                child.TrySetParent(Entity.GetComponentInParent<Game>().Deck.Entity);
            }

            Clear();
        }

        public void Clear()
        {
            foreach (IEntity unwantedChild in Entity.Children.ToList())
            {
                unwantedChild.Destroy();
            }
        }
    }
}
