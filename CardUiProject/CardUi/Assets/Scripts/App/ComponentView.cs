using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Api;
using UnityEngine;

namespace App
{
    public abstract class ComponentView<T> : MonoBehaviour where T : INotifyPropertyChanged
    {
        private IEntity Entity { get; set; }
        protected T Component { get; private set; }
        protected readonly List<IDisposable> Disposables = new List<IDisposable>(2);

        protected virtual void Start()
        {
            IView view = GetComponentInParent<IView>();
            Entity = view.Entity;
            view.Entity.Components.CollectionChanged += ComponentsOnCollectionChanged;
            UpdateComponentReference();
            ComponentOnPropertyChanged();
        }

        private void ComponentsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (UpdateComponentReference())
            {
                return;
            }

            gameObject.SetActive(Component != null);
        }

        private bool UpdateComponentReference()
        {
            var component = Entity.GetComponent<T>();
            if (Equals(component, Component))
            {
                return true;
            }

            if (Component != null && component != null)
            {
                Debug.LogWarning($"Replacing Component reference in {nameof(ComponentView<T>)}.");
            }

            if (Component != null)
            {
                Component.PropertyChanged -= ComponentOnPropertyChanged;
            }

            Component = component;

            if (Component != null)
            {
                Component.PropertyChanged += ComponentOnPropertyChanged;
            }

            //Let component update values because reference changed.
            ComponentOnPropertyChanged();
            return false;
        }

        private void ComponentOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this == null)
            {
                return;
            }
            ComponentOnPropertyChanged();
        }

        protected abstract void ComponentOnPropertyChanged();

        protected virtual void OnDestroy()
        {
            if (Entity != null)
            {
                Entity.Components.CollectionChanged -= ComponentsOnCollectionChanged;
            }

            foreach (IDisposable disposable in Disposables)
            {
                disposable.Dispose();
            }
        }
    }
}