using System;
using System.IO;
using Api;
using App;
using App.Utility;
using CardsAndPiles;
using CardsAndPiles.Components;
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

            var events = new SummerJam1Events();
            Context = new Context(events);
            Events.SubscribeToCardCreated(OnCardCreated);

            Context.SetPrefabsDirectory(Path.Combine("Assets", "External", "Library", "Prefabs"));
            IEntity game = Context.Root;

            SceneManager.LoadScene("MenuScene");

            Game = game.AddComponent<SummerJam1Game>();
        }


        private void OnCardCreated(object sender, CardCreatedEventArgs args)
        {
            if (SceneManager.GetActiveScene().name == "BattleScene")
            {
            }

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

            return null;
        }
    }
}