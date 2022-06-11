using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Api
{
    [Serializable]
    public class ChildrenCollection<T> : IChildrenCollection<T>, IList<T>
    {
        [ItemNotNull] [JsonProperty] private List<T> CollectionImplementation { get; set; } = new List<T>();
        public event NotifyCollectionChangedEventHandler CollectionChanged;


        public IEnumerator<T> GetEnumerator()
        {
            return CollectionImplementation.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item, Action invokeBeforeEvent)
        {
            CollectionImplementation.Add(item);
            invokeBeforeEvent?.Invoke();
            InvokeEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T> { item }));
        }

        private void InvokeEvent(NotifyCollectionChangedEventArgs args)
        {
            if (CollectionChanged != null)
            {
                // ReSharper disable once PossibleInvalidCastExceptionInForeachLoop
                foreach (NotifyCollectionChangedEventHandler handler in CollectionChanged.GetInvocationList())
                {
                    try
                    {
                        handler(this, args);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Error in the handler {0}: {1}", handler.Method.Name, e.Message);
                    }
                }
            }
        }

        public void Add(T item)
        {
            Add(item, null);
        }


        public void Clear()
        {
            var oldItems = CollectionImplementation.ToList();
            CollectionImplementation.Clear();
            InvokeEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems));
        }

        public bool Contains(T item)
        {
            return CollectionImplementation.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            CollectionImplementation.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item, Action invokeBeforeEvent)
        {
            bool removed = CollectionImplementation.Remove(item);
            if (removed)
            {
                invokeBeforeEvent?.Invoke();
                InvokeEvent(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove,
                    new List<T> { item }));
            }

            return removed;
        }

        public bool Remove(T item)
        {
            return Remove(item, null);
        }

        public int Count => CollectionImplementation.Count;
        public bool IsReadOnly => false;
        public int IndexOf(T item) => CollectionImplementation.FindIndex((child) => Equals(child, item));

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
            get => CollectionImplementation[index];
            set => CollectionImplementation[index] = value;
        }
    }
}