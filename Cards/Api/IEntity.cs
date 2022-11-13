using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Api
{
    public interface IEntity : INotifyPropertyChanged
    {
        Context Context { get; }
        int Id { get; }
        IEntity Parent { get; }
        IChildrenCollection<IEntity> Children { get; }
        IChildrenCollection<Component> Components { get; }
        LifecycleState State { get; }
        bool TrySetParent(IEntity parent);
        T GetComponent<T>();
        bool HasComponent<T>();
        List<T> GetComponents<T>();
        T GetComponentInParent<T>();
        T GetComponentInChildren<T>();
        List<T> GetComponentsInChildren<T>();
        T AddComponent<T>() where T : Component, new();
        IComponent AddComponent(Type componentType);
        bool RemoveComponent(Component toRemove);
        void Destroy();
        T GetOrAddComponent<T>() where T : Component, new();
        bool RemoveComponent<TType>() where TType : Component;
        T GetComponentInSelfOrParent<T>();
        bool CanSetParent(IEntity parent);
    }
}
