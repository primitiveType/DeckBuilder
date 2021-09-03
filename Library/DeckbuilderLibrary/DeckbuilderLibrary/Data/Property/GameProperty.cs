using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DeckbuilderLibrary.Data.Property
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
}