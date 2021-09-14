using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace DeckbuilderLibrary.Data
{
    [JsonObject]//Needs to be an object or else json convert tries to be clever.
    public class SerializableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        [JsonIgnore]
        private IDictionary<TKey, TValue> m_DictionaryImplementation = new Dictionary<TKey, TValue>();

        [JsonProperty] private List<TKey> keys { get; set; } = new List<TKey>();

        [JsonProperty] List<TValue> values { get; set; } = new List<TValue>();

        // save the dictionary to lists
        [OnSerializing]
        public void OnBeforeSerialize(StreamingContext context)
        {
            keys.Clear();
            values.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }

        // load dictionary from lists
        [OnDeserialized]
        public void OnAfterDeserialize(StreamingContext context)
        {
            this.Clear();

            if (keys.Count != values.Count)
                throw new System.Exception(string.Format(
                    "there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

            for (int i = 0; i < keys.Count; i++)
                this.Add(keys[i], values[i]);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return m_DictionaryImplementation.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)m_DictionaryImplementation).GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            m_DictionaryImplementation.Add(item);
        }

        public void Clear()
        {
            m_DictionaryImplementation.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return m_DictionaryImplementation.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            m_DictionaryImplementation.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return m_DictionaryImplementation.Remove(item);
        }

        public int Count => m_DictionaryImplementation.Count;

        public bool IsReadOnly => m_DictionaryImplementation.IsReadOnly;

        public void Add(TKey key, TValue value)
        {
            m_DictionaryImplementation.Add(key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return m_DictionaryImplementation.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            return m_DictionaryImplementation.Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return m_DictionaryImplementation.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get => m_DictionaryImplementation[key];
            set => m_DictionaryImplementation[key] = value;
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => m_DictionaryImplementation.Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => m_DictionaryImplementation.Values;

        public ICollection<TKey> Keys => m_DictionaryImplementation.Keys;

        public ICollection<TValue> Values => m_DictionaryImplementation.Values;
    }
}