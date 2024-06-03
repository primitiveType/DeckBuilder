using System.Collections.Generic;
using System.Collections.Specialized;

namespace Api
{
    public interface IChildrenCollection<out T> : INotifyCollectionChanged, IReadOnlyList<T>, IEnumerable<T>
    {
    }
}
