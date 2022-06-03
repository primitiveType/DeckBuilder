using System;
using System.Collections.Generic;
using System.Linq;
using Api;
using UnityEngine;

namespace Common
{
    public class View<T> : MonoBehaviour, IView<T> where T : IComponent
    {
        public Entity Entity { get; private set; }
        public T Model { get; private set; }

        public void SetModel(Entity entity)
        {
            Entity = entity;
            Model = entity.GetComponent<T>();
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
    }

    public interface IView
    {
        Entity Entity { get; }
    }

    public class PropertyChangedAttribute : Attribute
    {
        
    }
}