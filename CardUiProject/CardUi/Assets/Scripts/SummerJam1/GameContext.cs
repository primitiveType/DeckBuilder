using System;
using System.Collections.Generic;
using System.IO;
using Api;
using App.Utility;
using CardsAndPiles;
using External.UnityAsync.UnityAsync.Assets.UnityAsync;
using External.UnityAsync.UnityAsync.Assets.UnityAsync.AwaitInstructions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = App.Logger;

namespace SummerJam1
{
    public class GameContext : MonoBehaviourSingleton<GameContext>
    {
        private readonly List<IDisposable> Disposables = new List<IDisposable>();
        public Context Context { get; private set; }
        public SummerJam1Events Events => (SummerJam1Events)Context.Events;
        public Game Game { get; private set; }

        [SerializeField] private GameObject _EnableOnCreation;

        protected override void SingletonAwakened()
        {
            base.SingletonAwakened();
            Logging.Initialize(new Logger());
        }


        public void SaveGame()
        {
            string saveDataPath = GetSaveDataPath();
            string saveData = Serializer.Serialize(Context);
            File.WriteAllText(saveDataPath, saveData);
            SceneManager.LoadScene("StartScene");
        }

        private static string GetSaveDataPath()
        {
            string saveDataPath = Path.Combine(Application.persistentDataPath, "summerjam1Save.json");
            return saveDataPath;
        }

        public bool HasSaveData()
        {
            return File.Exists(GetSaveDataPath());
        }

        public void LoadGame()
        {
            Context = GetGameFromDisk();
            SubscribeToEvents();
            Game = Context.Root.GetComponent<Game>();


            LoadMenu();
        }

        private static Context GetGameFromDisk()
        {
            string data = File.ReadAllText(GetSaveDataPath());
            return Serializer.Deserialize<Context>(data);
        }

        private void CreateNewGame()
        {
            SummerJam1Events events = new SummerJam1Events();
            Context = new Context(events);
            SubscribeToEvents();

#if UNITY_EDITOR
            Debug.Log("We are in editor.");
            Context.SetPrefabsDirectory(Path.Combine("Assets", "External", "Library", "StreamingAssets"));
#else
            Debug.Log($"We are in a build. {Application.streamingAssetsPath}");
            Context.SetPrefabsDirectory(Application.streamingAssetsPath);
#endif
            IEntity game = Context.Root;


            Game = game.AddComponent<Game>();
        }

        private void SubscribeToEvents()
        {
            Disposables.Add(Events.SubscribeToCardPlayFailed(OnCardPlayFailed));
            Disposables.Add(Events.SubscribeToBattleStarted(OnBattleStarted));
            Disposables.Add(Events.SubscribeToBattleEnded(OnBattleEnded));
            Disposables.Add(Events.SubscribeToEntityKilled(OnEntityKilled));
        }

        private void OnBattleEnded(object sender, BattleEndedEventArgs item)
        {
            Debug.Log($"Battle ended. {item.Victory}.");
        }

        private void OnEntityKilled(object sender, EntityKilledEventArgs item)
        {
            Debug.Log($"Entity : {item.Entity.Id} killed by {item.Source.Id}");
        }

        private void OnBattleStarted(object sender, BattleStartedEventArgs item)
        {
            SceneManager.LoadScene("BattleScene");
        }

        private void OnCardPlayFailed(object sender, CardPlayFailedEventArgs item)
        {
            foreach (string itemReason in item.Reasons)
            {
                Debug.LogWarning(itemReason);
            }
        }

        private async void LoadMenu()
        {
            await new WaitForFrames(2);
            SceneManager.LoadScene("MapScene");
        }


        public async void StartOver()
        {
            Context.Root.Destroy();
            await new WaitForFrames(1); //allow a frame for views to destroy themselves
            CreateNewGame();
        }


        public void StartGame()
        {
            CreateNewGame();
            if (_EnableOnCreation != null)
            {
                _EnableOnCreation.SetActive(true);
            }

            SceneManager.LoadScene("Scenes/MapScene");
            // MusicAudo.Play();
        }

        public void StartBattle(int info)
        {
            Game.StartBattle();
        }
    }
}
