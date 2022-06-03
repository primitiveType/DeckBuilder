using System.Collections.Generic;
using System.Collections.Specialized;

namespace Api
{
    public interface IChildrenCollection<T> : INotifyCollectionChanged, IReadOnlyList<T>
    {
    }
}