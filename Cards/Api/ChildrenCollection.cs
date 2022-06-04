using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using JetBrains.Annotations;

namespace Api
{
    public static class CollectionExtensions
    {
        public static void Shuffle(this ICollection collection)
        {
        }
    }

    public class ChildrenCollection<T> : IChildrenCollection<T>, IList<T>
    {
        [ItemNotNull] private List<T> m_CollectionImplementation = new List<T>();
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
        public bool IsReadOnly => false;
        public int IndexOf(T item) => m_CollectionImplementation.FindIndex((child) => Equals(child, item));

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public T this[int index]
        {
            get => m_CollectionImplementation[index];
            set => m_CollectionImplementation[index] = value;
        }
    }
}