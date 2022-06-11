using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Api;
using UnityEngine;
using IComponent = Api.IComponent;

namespace App
{
    public class View<T> : MonoBehaviour, IView<T> where T : IComponent
    {
        public IEntity Entity { get; private set; }
        public T Model { get; private set; }
        protected List<IDisposable> Disposables { get; } = new List<IDisposable>(2);

        public void SetModel(IEntity entity)
        {
            Entity = entity;
            Model = entity.GetComponent<T>();
            if (Model == null)
            {
                Debug.LogWarning($"Failed to find model {typeof(T).Name} on Entity Component.");
                enabled = false;
                return;
            }

            AttachListeners();
            OnInitialized();
            Entity.PropertyChanged += OnEntityDestroyed;
        }

        private void OnEntityDestroyed(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == nameof(IEntity.State) &&
                Entity.State == LifecycleState.Destroyed)
            {
                Disposables.Add(AnimationQueue.Instance.Enqueue(Destroy));
            }
        }

        private void Destroy()
        {
            if (this != null && gameObject != null)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnInitialized()
        {
        }

        protected virtual void Start()
        {
            if (Entity == null)
            {
                IView parentView = GetComponentsInParent<IView>().FirstOrDefault(item => !ReferenceEquals(item, this) && item.Entity != null);
                if (parentView?.Entity != null)
                {
                    SetModel(parentView.Entity);
                }
            }
        }

        private List<PropertyChangedEventHandler> EventHandlers = new List<PropertyChangedEventHandler>();

        private void AttachListeners()
        {
            foreach (PropertyListenerInfo method in ReflectionService.GetPropertyListeners(GetType()))
            {
                PropertyChangedEventHandler action = (sender, args) =>
                {
                    try
                    {
                        if (method.Filter == null || method.Filter == args.PropertyName)
                        {
                            method.MethodInfo.Invoke(this, new[] { sender, args });
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Caught exception executing event! {e.Message}", this);
                    }
                };
                Model.PropertyChanged += action;
                EventHandlers.Add(action);
                try
                {
                    method.MethodInfo.Invoke(this, new object[] { this, new PropertyChangedEventArgs(method.Filter) });
                }
                catch (Exception e)
                {
                    Debug.LogError($"Caught exception executing event! {e.Message}", this);
                }
            }
        }

        protected virtual void OnDestroy()
        {
            foreach (PropertyChangedEventHandler action in EventHandlers)
            {
                Model.PropertyChanged -= action;
            }

            if (Entity != null)
            {
                Entity.PropertyChanged -= OnEntityDestroyed;
            }

            foreach (IDisposable disposable in Disposables)
            {
                disposable.Dispose();
            }
        }
    }

    public interface IView
    {
        IEntity Entity { get; }
    }
}