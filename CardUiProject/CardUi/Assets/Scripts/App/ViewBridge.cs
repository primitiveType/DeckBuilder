using System.Collections.Specialized;
using Api;
using UnityEngine;
using Component = Api.Component;

namespace Common
{
   public abstract class ViewBridge<T, TThis> : Component, IGameObject where T : IComponent where TThis : Component, new()
   {
       public abstract GameObject Prefab { get; }
       public GameObject gameObject { get; private set; }
   
       protected override void Initialize()
       {
           base.Initialize();
           T card = Entity.GetComponent<T>();
           if (card != null)
           {
               MakeCard(card);
           }
   
   
           Entity.Children.CollectionChanged += ChildrenOnCollectionChanged;
           foreach (IEntity child in Entity.Children)
           {
               AddBridgeIfMissing(child);
           }
       }
   
       private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
       {
           if (e.Action == NotifyCollectionChangedAction.Add)
           {
               foreach (IEntity item in e.NewItems)
               {
                   AddBridgeIfMissing(item);
               }
           }
       }
   
       private static void AddBridgeIfMissing(IEntity item)
       {
           if (item.GetComponent<TThis>() == null)
           {
               item.AddComponent<TThis>();
           }
       }
   
       private void MakeCard(T cardModel)
       {
           View<T> card = Object.Instantiate(Prefab).GetComponent<View<T>>();
           gameObject = card.gameObject;
           card.SetModel(cardModel.Entity);
       }
   }

}