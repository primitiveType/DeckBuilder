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
}