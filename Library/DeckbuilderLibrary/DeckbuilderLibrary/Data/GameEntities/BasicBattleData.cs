using System.Collections.Generic;
using DeckbuilderLibrary.Data.GameEntities.Actors;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public class BasicBattleData : BattleData
    {
        public override List<Enemy> GetStartingEnemies()
        {
            BasicEnemy enemy = Context.CreateActor<BasicEnemy>(100, 0);
            return new List<Enemy> { enemy };
        }
    }
}