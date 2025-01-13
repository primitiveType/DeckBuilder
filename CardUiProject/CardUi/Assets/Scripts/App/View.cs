using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Api;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using IComponent = Api.IComponent;

namespace App
{
    public class View<T> : MonoBehaviour, IView<T>
    {
        [SerializeField] private int DEBUG_entity;
        [SerializeField] private int DEBUG_component;
        [SerializeField] private bool required = true;

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
                if (_model != null)
                {
                    Debug.LogWarning($"Model changing from {_model.GetHashCode()} to {value.GetHashCode()}");
                }
                else
                {
                    Debug.LogWarning($"Model set to {value.GetHashCode()}");
                }

                _model = value;

                DEBUG_component = value != null ? value.GetHashCode() : -1;


                OnPropertyChanged();
            }
        }

        protected List<IDisposable> Disposables { get; } = new List<IDisposable>(2);

        public void SetModel(IEntity entity)
        {
            InternalSetEntity(entity);
            var model = entity.GetComponent<T>();
            InternalSetModel(model);
        }

        private void InternalSetEntity(IEntity entity)
        {
            if (Entity != null)
            {
                Entity.PropertyChanged -= OnEntityDestroyed;
                Logging.LogWarning("SetModel called on view that already had been populated.");
            }

            Entity = entity;
            Entity.PropertyChanged += OnEntityDestroyed;
        }

        private void InternalSetModel(T model)
        {
            Model = model;
            if (required && Model == null)
            {
                Debug.LogError($"Failed to find model {typeof(T).Name} on Entity Component.", gameObject);
                enabled = false;
                return;
            }

            if (Model != null)
            {
                AttachListeners();
            }

            OnInitialized();
        }

        public void SetModel(IComponent component)
        {
            InternalSetEntity(component.Entity);
            InternalSetModel((T)component);
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
            ParentView = GetComponentsInParent<IView>()
                .FirstOrDefault(item => !ReferenceEquals(item, this));
            if (Entity == null)
            {
                var entity = GetEntityForView();
                if (entity != null)
                {
                    SetModel(entity);
                }
                else
                {
                    // Debug.LogWarning("Parent view has no entity. Listening for it to be populated...", gameObject);
                    ParentView.PropertyChanged += ParentViewOnPropertyChanged;
                }
            }
        }

        protected virtual IEntity GetEntityForView()
        {
            if (ParentView == null)
            {
                Debug.LogWarning($"Failed to find Parent View for component. {this.GetType().Name}", gameObject);
                enabled = false;
                return null;
            }

            return ParentView?.Entity;
        }

        private void ParentViewOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IView.Entity))
            {
                // Debug.Log("Parent view populated entity.");
                ParentView.PropertyChanged -= ParentViewOnPropertyChanged;
                SetModel(ParentView.Entity);
            }
        }

        private List<PropertyChangedEventHandler> EventHandlers = new();
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
                            if (method.MethodInfo.GetParameters().Length > 0)
                            {
                                method.MethodInfo.Invoke(this, new[] { sender, args });
                            }
                            else
                            {
                                method.MethodInfo.Invoke(this, null);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.LogError($"Caught exception executing event! {e.Message} : {e.InnerException.StackTrace}",
                            this);
                    }
                };
                ((INotifyPropertyChanged)Model).PropertyChanged += action;
                EventHandlers.Add(action);
                try
                {
                    action.Invoke(this, new PropertyChangedEventArgs(method.Filter));
                }
                catch (Exception e)
                {
                    Debug.LogError($"Caught exception executing event! {e}", this);
                    Debug.LogException(e);
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