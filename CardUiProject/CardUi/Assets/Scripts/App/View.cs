using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Api;
using JetBrains.Annotations;
using UnityEngine;

namespace App
{
    public class View<T> : MonoBehaviour, IView<T>
    {
        [SerializeField] private int DEBUG_entity; 
        public IEntity Entity
        {
            get => _entity;
            private set
            {
                _entity = value;
                DEBUG_entity = _entity.Id;
                OnPropertyChanged();
            }
        }

        public T Model
        {
            get => _model;
            private set
            {
                _model = value;
                OnPropertyChanged();
            }
        }

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
                ParentView = GetComponentsInParent<IView>().FirstOrDefault(item => !ReferenceEquals(item, this) && item.Entity != null);
                if (ParentView == null)
                {
                    Debug.LogWarning($"Failed to find Parent View for component.", gameObject);
                    enabled = false;
                    return;
                }

                if (ParentView?.Entity != null)
                {
                    SetModel(ParentView.Entity);
                }
                else
                {
                    ParentView.PropertyChanged += ParentViewOnPropertyChanged;
                }
            }
        }

        private void ParentViewOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IView.Entity))
            {
                ParentView.PropertyChanged -= ParentViewOnPropertyChanged;
                SetModel(ParentView.Entity);
            }
        }

        private List<PropertyChangedEventHandler> EventHandlers = new List<PropertyChangedEventHandler>();
        private IEntity _entity;
        private T _model;
        private IView ParentView { get; set; }

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
                ((INotifyPropertyChanged)Model).PropertyChanged += action;
                EventHandlers.Add(action);
                try
                {
                    action.Invoke(this,  new PropertyChangedEventArgs(method.Filter) );
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
                ((INotifyPropertyChanged)Model).PropertyChanged -= action;
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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface IView : INotifyPropertyChanged
    {
        IEntity Entity { get; }
    }
}
