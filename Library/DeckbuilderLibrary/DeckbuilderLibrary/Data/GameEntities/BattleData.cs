using System.Collections.Generic;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public abstract class BattleData : GameEntity
    {
        /// <summary>
        /// This method returns the list of enemies that will be added to the context.
        /// These get added before the battle begin event, and it gives an opportunity to initialize any necessary state
        /// before that event. For example, an enemy might start with vulnerable, or with half health.
        /// </summary>
        /// <returns></returns>
        public abstract List<Enemy> GetStartingEnemies();
    }
}