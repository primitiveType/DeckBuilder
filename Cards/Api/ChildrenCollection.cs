using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using JetBrains.Annotations;

namespace Api
{
    public class ChildrenCollection<T> : INotifyCollectionChanged, IChildrenCollection<T>, ICollection<T>
    {
        [ItemNotNull] private ICollection<T> m_CollectionImplementation = new Collection<T>();
        public event NotifyCollectionChangedEventHandler CollectionChanged;


        public IEnumerator<T> GetEnumerator()
        {
            return m_CollectionImplementation.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item, Action invokeBeforeEvent)
        {
            m_CollectionImplementation.Add(item);
            invokeBeforeEvent?.Invoke();
            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T> { item }));
        }

        public void Add(T item)
        {
            Add(item, null);
        }


        public void Clear()
        {
            var oldItems = m_CollectionImplementation.ToList();
            m_CollectionImplementation.Clear();
            CollectionChanged?.Invoke(this,
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems));
        }

        public bool Contains(T item)
        {
            return m_CollectionImplementation.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_CollectionImplementation.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item, Action invokeBeforeEvent)
        {
            bool removed = m_CollectionImplementation.Remove(item);
            if (removed)
            {
                invokeBeforeEvent?.Invoke();
                CollectionChanged?.Invoke(this,
                    new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new List<T> { item }));
            }

            return removed;
        }

        public bool Remove(T item)
        {
            return Remove(item, null);
        }

        public int Count => m_CollectionImplementation.Count;
        public bool IsReadOnly => m_CollectionImplementation.IsReadOnly;
    }
}