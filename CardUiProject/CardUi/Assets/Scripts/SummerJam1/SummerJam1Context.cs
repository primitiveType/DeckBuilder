using System;
using System.IO;
using System.Threading.Tasks;
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

        [SerializeField] private AudioSource HitAudio;
        [SerializeField] private AudioSource CardAudio;
        [SerializeField] private AudioSource ButtonAudio;

        protected override void Awake()
        {
            base.Awake();

            Setup();
        }

        private void Setup()
        {
            var events = new SummerJam1Events();
            Context = new Context(events);
            Events.SubscribeToCardCreated(OnCardCreated);
            Events.SubscribeToRelicCreated(OnRelicCreated);
            Events.SubscribeToDamageDealt(OnDamageDealt);
            Events.SubscribeToCardPlayed(OnCardPlayed);

#if UNITY_EDITOR
            Debug.Log("We are in editor.");
            Context.SetPrefabsDirectory(Path.Combine("Assets", "External", "Library", "StreamingAssets"));
#else
            Debug.Log($"We are in a build. {Application.streamingAssetsPath}");
            Context.SetPrefabsDirectory(Application.streamingAssetsPath);
#endif
            IEntity game = Context.Root;


            Game = game.AddComponent<SummerJam1Game>();
            LoadMenu();
        }

        private async void LoadMenu()
        {
            await new WaitForFrames(1);
            SceneManager.LoadScene("MenuScene");
        }

        private void OnCardPlayed(object sender, CardPlayedEventArgs item)
        {
            AnimationQueue.Instance.Enqueue(() => CardAudio.PlayOneShot(CardAudio.clip));
        }

        private void OnDamageDealt(object sender, DamageDealtEventArgs item)
        {
            AnimationQueue.Instance.Enqueue(() => HitAudio.PlayOneShot(HitAudio.clip));
        }

        public void ButtonClicked()
        {
            ButtonAudio.PlayOneShot(ButtonAudio.clip);
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
            await new WaitForFrames(1); //allow a frame for views to destroy themselves
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
