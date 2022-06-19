using System;
using System.IO;
using Api;
using App;
using App.Utility;
using CardsAndPiles;
using CardsAndPiles.Components;
using External.UnityAsync.UnityAsync.Assets.UnityAsync;
using External.UnityAsync.UnityAsync.Assets.UnityAsync.AwaitInstructions;
using SummerJam1.Units;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SummerJam1
{
    public class SummerJam1Context : MonoBehaviourSingleton<SummerJam1Context>
    {
        public Context Context { get; private set; }
        public SummerJam1Events Events => (SummerJam1Events)Context.Events;
        public SummerJam1Game Game { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            Setup();
        }

        private async void Setup()
        {
            var events = new SummerJam1Events();
            Context = new Context(events);
            Events.SubscribeToCardCreated(OnCardCreated);
            Events.SubscribeToRelicCreated(OnRelicCreated);

            Context.SetPrefabsDirectory(Path.Combine("Assets", "External", "Library", "Prefabs"));
            IEntity game = Context.Root;


            Game = game.AddComponent<SummerJam1Game>();
            await new WaitForFrames(1);
            SceneManager.LoadScene("MenuScene");
        }


        private void OnRelicCreated(object sender, RelicCreatedEventArgs args)
        {
            IEntity entity = args.Relic;
            CreateView(entity, SummerJam1CardFactory.Instance.RelicPrefab);
        }


        private void OnCardCreated(object sender, CardCreatedEventArgs args)
        {
            var entity = args.CardId;
            CreateView(entity, SummerJam1CardFactory.Instance.CardPrefab);
        }

        public static GameObject CreateView(IEntity entity, GameObject prefab)
        {
            GameObject unitView = Instantiate(prefab);
            unitView.transform.localPosition = Vector3.one * 10_000;
            unitView.GetComponent<ISetModel>().SetModel(entity);
            entity.GetOrAddComponent<SummerJam1ModelViewBridge>().gameObject = unitView;

            return unitView;
        }

        public async void StartOver()
        {
            Context.Root.Destroy();
            await new WaitForFrames(1);//allow a frame for views to destroye themselves
            SceneManager.LoadScene("Main");
            Setup();
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
    }
}
