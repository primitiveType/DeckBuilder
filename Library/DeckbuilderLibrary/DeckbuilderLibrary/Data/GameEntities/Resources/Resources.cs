using DeckbuilderLibrary.Data.GameEntities.Actors;
using Newtonsoft.Json;

namespace DeckbuilderLibrary.Data.GameEntities.Resources
{
    public class Resources : GameEntity
    {
        [JsonProperty]
        private SerializableDictionary<string, IResource> ResourcesDictionary { get; set; } =
            new SerializableDictionary<string, IResource>();

        [JsonIgnore] public Actor Owner { get; set; }

        public int GetResourceAmount<T>() where T : Resource<T>
        {
            if (ResourcesDictionary.TryGetValue(typeof(T).Name, out IResource val))
            {
                return val.Amount;
            }

            return 0;
        }

        public void AddResource<T>(int amount) where T : Resource<T>, IResource, new()
        {
            if (!HasResource<T>())
            {
                ResourcesDictionary.Add(typeof(T).Name, Context.CreateResource<T>(Owner, amount));
                return;
            }

            ResourcesDictionary[typeof(T).Name].Add(amount);
        }

        public void SubtractResource<T>(int amount) where T : Resource<T>, IResource, new()
        {
            if (!HasResource<T>())
            {
                ResourcesDictionary.Add(typeof(T).Name, Context.CreateResource<T>(Owner, 0));
                return;
            }

            ResourcesDictionary[typeof(T).Name].Subtract(amount);
        }

        public void SetResource<T>(int amount) where T : Resource<T>, IResource, new()
        {
            if (!HasResource<T>())
            {
                ResourcesDictionary.Add(typeof(T).Name, Context.CreateResource<T>(Owner, amount));
                return;
            }

            ResourcesDictionary[typeof(T).Name].Set(amount);
        }

        public bool HasResource<T>() where T : Resource<T>, IResource, new()
        {
            return ResourcesDictionary.ContainsKey(typeof(T).Name);
        }
    }
}