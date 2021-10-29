using DeckbuilderLibrary.Data.GameEntities;

namespace DeckbuilderLibrary.Data
{
    public class EntityCreatedArgs
    {
        public readonly IGameEntity Entity;

        public EntityCreatedArgs(IGameEntity entity)
        {
            Entity = entity;
        }
    }
    
    public class EntityDestroyedArgs
    {
        public readonly IGameEntity Entity;

        public EntityDestroyedArgs(IGameEntity entity)
        {
            Entity = entity;
        }
    }
}