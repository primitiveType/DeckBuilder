namespace SummerJam1
{
    public class BountyBattleFactory : IBattleFactory
    {
        public void StartBattle(Game game, int difficulty)
        {
            var monsters = game.GetBattlePrefabs(10 + difficulty, 15 + difficulty);
               
        }
    }
}