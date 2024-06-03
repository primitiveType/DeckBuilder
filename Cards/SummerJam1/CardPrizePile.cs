using System.Linq;
using Api;
using CardsAndPiles;

namespace SummerJam1
{
    public class CardPrizePile : PrizePile
    {
        [OnBattleEnded]
        private void OnBattleEnded(object sender, BattleEndedEventArgs args)
        {
            if (args.Victory)
            {
                //Player.Entity.TrySetParent(TempPlayerSlot.Entity);
                //SetupRandomPrizePile();
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
                var prefab = child.GetComponent<SourcePrefab>().Prefab;

                var game = Entity.GetComponentInParent<Game>();

                Context.CreateEntity(game.Deck.Entity, prefab);

                if (game.Battle != null)
                {
                    Context.CreateEntity(game.Battle.Hand.Entity, prefab);
                }
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
