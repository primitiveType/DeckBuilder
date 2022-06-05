using System.Collections.Specialized;
using System.ComponentModel;
using Api;
using Common;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace App
{
    public abstract class ComponentView<T> : MonoBehaviour where T : INotifyPropertyChanged
    {
        private IEntity Entity { get; set; }
        protected T Component { get; private set; }

        private void Start()
        {
            IView view = GetComponentInParent<IView>();
            Entity = view.Entity;
            view.Entity.Components.CollectionChanged += ComponentsOnCollectionChanged;
            UpdateComponentReference();
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
            ComponentOnPropertyChanged();
        }

        protected abstract void ComponentOnPropertyChanged();

        protected virtual void OnDestroy()
        {
            Entity.Components.CollectionChanged -= ComponentsOnCollectionChanged;
        }
    }
}