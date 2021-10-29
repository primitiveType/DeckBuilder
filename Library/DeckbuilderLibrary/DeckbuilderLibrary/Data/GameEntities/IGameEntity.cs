using System;
using System.ComponentModel;

namespace DeckbuilderLibrary.Data.GameEntities
{
    public interface IGameEntity
    {
        int Id { get; }
        IContext Context { get; }
        void AddListener(Action<object, PropertyChangedEventArgs> action);
        event EntityDestroyed DestroyedEvent;
    }
}