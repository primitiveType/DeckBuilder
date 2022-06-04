namespace Api
{
    public class Context
    {
        private int NextId { get; set; }
        public IEntity CreateEntity(IEntity parent = null)
        {
            var entity = new Entity();
            entity.Initialize(this, NextId ++);
            entity.SetParent(parent);
            
            return entity;
        }
    }
}