using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class Properties
    {
        //kinda seems like all we need are ints tbh
        public SerializableDictionary<string, int> Ints { get; private set; } = new SerializableDictionary<string, int>();

        public Properties()
        {
            
        }
        //copy ctor
        public Properties(SerializableDictionary<string, int> ints)
        {
            foreach (KeyValuePair<string, int> pair in ints)
            {
                Ints.Add(pair.Key, pair.Value);
            }
        }
    }
}