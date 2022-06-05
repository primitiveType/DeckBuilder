using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Api;
using UnityEngine;
using IComponent = Api.IComponent;

namespace Common
{
    public class View<T> : MonoBehaviour, IView<T> where T : IComponent
    {
        public IEntity Entity { get; private set; }
        public T Model { get; private set; }

        [SerializeField] private bool m_Initialized_Tester;

        public void SetModel(IEntity entity)
        {
            m_Initialized_Tester = true;
            Entity = entity;
            Model = entity.GetComponent<T>();
            if (Model == null)
            {
                throw new NullReferenceException($"Failed to find model {typeof(T).Name} on Entity Component");
            }
                
            AttachListeners();
            OnInitialized();
        }

        protected virtual void OnInitialized()
        {
        }

        protected virtual void Start()
        {
            if (Entity == null)
            {
                IView parentView = GetComponents<IView>().Where(item => item != this).FirstOrDefault();
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
                method.MethodInfo.Invoke(this, new object[] { this, new PropertyChangedEventArgs(method.Filter) });
            }
        }

        protected void OnDestroy()
        {
            foreach (PropertyChangedEventHandler action in EventHandlers)
            {
                Model.PropertyChanged -= action;
            }
        }
    }

    public interface IView
    {
        IEntity Entity { get; }
    }
}