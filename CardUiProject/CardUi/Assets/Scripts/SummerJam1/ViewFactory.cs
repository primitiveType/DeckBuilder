using System;
using System.Collections.Generic;
using Api;
using App;
using App.Utility;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Units;
using UnityEngine;

namespace SummerJam1
{
    public class ViewFactory : MonoBehaviourSingleton<ViewFactory>
    {
        private List<IDisposable> Disposables = new List<IDisposable>();

        protected override void SingletonAwakened()
        {
            base.SingletonAwakened();

            Disposables.Add(GameContext.Instance.Events.SubscribeToCardCreated(OnCardCreated));
            Disposables.Add(GameContext.Instance.Events.SubscribeToRelicCreated(OnRelicCreated));
        }

        private void OnGameLoadedOrSomething()
        {
            foreach (Card childCard in GameContext.Instance.Game.Entity.GetComponentsInChildren<Card>())
            {
                CreateView(childCard.Entity, SummerJam1CardFactory.Instance.CardPrefab);
            }

            foreach (RelicComponent relic in GameContext.Instance.Game.Entity.GetComponentsInChildren<RelicComponent>())
            {
                CreateView(relic.Entity, SummerJam1CardFactory.Instance.RelicPrefab);
            }
        }

        private void OnRelicCreated(object sender, RelicCreatedEventArgs args)
        {
            IEntity entity = args.Relic;
            CreateView(entity, SummerJam1CardFactory.Instance.RelicPrefab);
        }


        private void OnCardCreated(object sender, CardCreatedEventArgs args)
        {
            IEntity entity = args.CardId;
            CreateView(entity, SummerJam1CardFactory.Instance.CardPrefab);
        }

        public GameObject CreateGameObjectForModel(SummerJam1ModelViewBridge summerJam1ModelViewBridge)
        {
            if (summerJam1ModelViewBridge.Entity.GetComponent<Unit>() != null)
            {
                return CreateView(summerJam1ModelViewBridge.Entity, SummerJam1UnitFactory.Instance.UnitPrefab);
            }

            if (summerJam1ModelViewBridge.Entity.GetComponent<Card>() != null)
            {
                return CreateView(summerJam1ModelViewBridge.Entity, SummerJam1CardFactory.Instance.CardPrefab);
            }

            if (summerJam1ModelViewBridge.Entity.GetComponent<RelicComponent>() != null)
            {
                return CreateView(summerJam1ModelViewBridge.Entity, SummerJam1CardFactory.Instance.RelicPrefab);
            }


            return null;
        }

        public static GameObject CreateView(IEntity entity, GameObject prefab)
        {
            GameObject unitView = Instantiate(prefab);
            unitView.transform.localPosition = Vector3.one * 10_000;
            unitView.GetComponent<ISetModel>().SetModel(entity);
            entity.GetOrAddComponent<SummerJam1ModelViewBridge>().gameObject = unitView;

            return unitView;
        }
    }
}