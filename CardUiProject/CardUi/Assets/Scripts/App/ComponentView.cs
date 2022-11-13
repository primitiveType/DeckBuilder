using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using Api;
using UnityEngine;

namespace App
{
    public abstract class ComponentView<T> : MonoBehaviour
    {
        protected IEntity Entity { get; private set; }
        protected T Component { get; private set; }
        protected readonly List<IDisposable> Disposables = new List<IDisposable>(2);
        protected IView View { get; private set; }

        [SerializeField] protected bool m_HideIfNull = false;
        protected virtual bool m_DisableComponentIfNull => false;
        [SerializeField] protected GameObject m_VisibilityObject;

        protected GameObject VisibilityObject => m_VisibilityObject ? m_VisibilityObject : gameObject;

        protected virtual void Start()
        {
            View = GetComponentInParent<IView>();
            UpdateVisibility(true);
            if (View.Entity == null)
            {
                View.PropertyChanged += ViewOnPropertyChanged;
                return;
            }

            GetEntity(View);
        }

        private void GetEntity(IView view)
        {
            Entity = view.Entity;
            view.Entity.Components.CollectionChanged += ComponentsOnCollectionChanged;
            UpdateComponentReference();
            ComponentOnPropertyChanged();
        }

        private void ViewOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IView.Entity))
            {
                View.PropertyChanged -= ViewOnPropertyChanged;
                GetEntity(View);
            }
        }

        private void ComponentsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var changed = UpdateComponentReference();
            if (!changed)
            {
                return;
            }

            UpdateVisibility(false);
        }

        /// <summary>
        /// Returns true if the reference changes.
        /// </summary>
        /// <returns></returns>
        private bool UpdateComponentReference()
        {
            var component = Entity.GetComponent<T>();
            if (Equals(component, Component))
            {
                return false;
            }


            if (Component != null && component != null)
            {
                Debug.LogWarning($"Replacing Component reference in {nameof(ComponentView<T>)}.");
            }

            if (Component != null)
            {
                ((INotifyPropertyChanged)Component).PropertyChanged -= ComponentOnPropertyChanged;
            }

            Component = component;
            UpdateVisibility(false);

            if (Component != null)
            {
                ((INotifyPropertyChanged)Component).PropertyChanged += ComponentOnPropertyChanged;
            }

            //Let component update values because reference changed.
            ComponentOnPropertyChanged();
            return true;
        }

        private void ComponentOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this == null)
            {
                return;
            }


            ComponentOnPropertyChanged();
        }

        private void UpdateVisibility(bool immediate)
        {
            bool visible = Component != null || !m_HideIfNull;
            bool disabled = Component == null && m_DisableComponentIfNull;
            if (immediate)
            {
                VisibilityObject.SetActive(visible);
                enabled = !disabled;
            }
            else
            {
                Disposables.Add(AnimationQueue.Instance.Enqueue(() => VisibilityObject.SetActive(visible)));
                enabled = !disabled;
            }
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

    // public abstract class MultiComponentView<T, TView> : MonoBehaviour where T : INotifyPropertyChanged
    // {
    //     private IEntity Entity { get; set; }
    //     protected T Component { get; private set; }
    //     protected readonly List<IDisposable> Disposables = new List<IDisposable>(2);
    //     private Dictionary<T, TView> ViewsByModel = new Dictionary<T, TView>();
    //
    //     protected virtual void Start()
    //     {
    //         IView view = GetComponentInParent<IView>();
    //         Entity = view.Entity;
    //         view.Entity.Components.CollectionChanged += ComponentsOnCollectionChanged;
    //         UpdateComponentReference();
    //         ComponentOnPropertyChanged();
    //     }
    //
    //     private void ComponentsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    //     {
    //         if (UpdateComponentReference())
    //         {
    //             return;
    //         }
    //
    //         gameObject.SetActive(Component != null);
    //     }
    //
    //     private bool UpdateComponentReference()
    //     {
    //         var component = Entity.GetComponent<T>();
    //         if (Equals(component, Component))
    //         {
    //             return true;
    //         }
    //
    //         if (Component != null && component != null)
    //         {
    //             Debug.LogWarning($"Replacing Component reference in {nameof(ComponentView<T>)}.");
    //         }
    //
    //         if (Component != null)
    //         {
    //             Component.PropertyChanged -= ComponentOnPropertyChanged;
    //         }
    //
    //         Component = component;
    //
    //         if (Component != null)
    //         {
    //             Component.PropertyChanged += ComponentOnPropertyChanged;
    //         }
    //
    //         //Let component update values because reference changed.
    //         ComponentOnPropertyChanged();
    //         return false;
    //     }
    //
    //     private void ComponentOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    //     {
    //         if (this == null)
    //         {
    //             return;
    //         }
    //
    //         ComponentOnPropertyChanged();
    //     }
    //
    //     protected abstract void ComponentOnPropertyChanged();
    //
    //     protected virtual void OnDestroy()
    //     {
    //         if (Entity != null)
    //         {
    //             Entity.Components.CollectionChanged -= ComponentsOnCollectionChanged;
    //         }
    //
    //         foreach (IDisposable disposable in Disposables)
    //         {
    //             disposable.Dispose();
    //         }
    //     }
    // }
}
