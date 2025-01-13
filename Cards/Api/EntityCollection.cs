using System.Collections.Generic;
using System.Linq;

namespace Api
{
    public class EntityCollection : ChildrenCollection<IEntity>
    {
        public void DestroyRecursive()
        {
            List<IEntity> oldItems = CollectionImplementation.ToList();
            foreach (var child in oldItems)
            {
                child.Children.DestroyRecursive();
                child.Destroy();
            }
            CollectionImplementation.Clear();
        }
    }
}