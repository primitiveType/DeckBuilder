using System.Collections.Generic;

namespace SummerJam1
{
    public class BountyBattleFactory : IBattleFactory
    {
        public void StartBattle(Game game, int difficulty)
        {
            List<string> monsters = game.GetBattlePrefabs(5, 5);
        }
    }
}
