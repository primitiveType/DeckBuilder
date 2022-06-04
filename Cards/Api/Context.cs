using System;

namespace Api
{
    public class Context
    {
        private int NextId { get; set; }
        
        //create entity
        //initialize it
        //set its parent
        //add components
        //initialize components
        
        
        //Parent should not be available in initialize. Instead, listen to parent changed.
        //Components can be added at any time and will get initialized.
        
        
        public IEntity CreateEntity(IEntity parent = null, Action<IEntity> setup = null)
        {
            var entity = new Entity();
            entity.Initialize(this, NextId ++);

            setup?.Invoke(entity);
            entity.TrySetParent(parent);
            return entity;
        }
    }
}