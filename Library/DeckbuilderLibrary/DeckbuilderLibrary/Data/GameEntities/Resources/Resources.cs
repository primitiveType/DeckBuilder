using System;
using System.Collections.Generic;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using Newtonsoft.Json;

namespace DeckbuilderLibrary.Data.GameEntities.Resources
{
    public abstract class GameProperty<T> : IEquatable<T>
    {
        public T Value { get; set; }

        public GameProperty(T value)
        {
            Value = value;
        }

        [JsonConstructor]
        protected GameProperty()
        {
        }

        protected bool Equals(GameProperty<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public bool Equals(T other)
        {
            return EqualityComparer<T>.Default.Equals(Value, other);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((BattleProperty<T>)obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Value);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class TurnProperty<T> : GameProperty<T>
    {
        public TurnProperty(T value) : base(value)
        {
            GameContext.CurrentContext.Events.TurnEnded += OnTurnEnded;
        }

        private void OnTurnEnded(object sender, TurnEndedEventArgs args)
        {
            Value = default(T);
        }

        public TurnProperty()
        {
            GameContext.CurrentContext.Events.TurnEnded += OnTurnEnded;
        }
    }

    public class BattleProperty<T> : GameProperty<T>
    {
        public BattleProperty(T value) : base(value)
        {
            GameContext.CurrentContext.Events.BattleEnded += OnBattleEnded;
        }

        public BattleProperty()
        {
            GameContext.CurrentContext.Events.BattleEnded += OnBattleEnded;
        }

        private void OnBattleEnded(object sender, BattleEndedEventArgs args)
        {
            Value = default(T);
        }
    }

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