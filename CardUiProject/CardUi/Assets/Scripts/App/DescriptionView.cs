using System.Collections.Specialized;
using System.ComponentModel;
using CardsAndPiles.Components;
using TMPro;
using UnityEngine;

namespace App
{
    public class DescriptionView : MultipleComponentView<IDescription>
    {
        [SerializeField] private TMP_Text m_Text;


        protected override void OnComponentPropertyChanged()
        {
            string description = "";
            foreach (IDescription component in Entity.GetComponents<IDescription>())
            {
                description += component.Description + System.Environment.NewLine;
            }

            m_Text.text = description;
        }
    }

    public abstract class MultipleComponentView<TComponent> : View<TComponent> where TComponent : Api.IComponent
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Entity.Components.CollectionChanged += ComponentsOnCollectionChanged;
            foreach (TComponent component in Entity.GetComponents<TComponent>())
            {
                AddListenerForComponent(component);
            }

            OnComponentPropertyChanged();
        }

        private void ComponentsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (TComponent eNewItem in e.NewItems)
                {
                    AddListenerForComponent(eNewItem);
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (TComponent eOldItem in e.OldItems)
                {
                    RemoveListenerForComponent(eOldItem);
                }
            }
        }

        private void AddListenerForComponent(TComponent eNewItem)
        {
            eNewItem.PropertyChanged += OnComponentPropertyChanged;
        }

        private void RemoveListenerForComponent(TComponent oldItem)
        {
            oldItem.PropertyChanged -= OnComponentPropertyChanged;
        }

        private void OnComponentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnComponentPropertyChanged();
        }

        protected abstract void OnComponentPropertyChanged();
    }
}
