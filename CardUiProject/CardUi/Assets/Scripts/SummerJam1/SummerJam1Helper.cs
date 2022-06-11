using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Api;
using App;
using CardsAndPiles;
using Common;
using SummerJam1.Units;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SummerJam1
{
    public class SummerJam1Helper : MonoBehaviour //essentially a view<Battle>
    {
        [SerializeField] private GameObject m_UnitPrefab;
        public GameObject UnitPrefab => m_UnitPrefab;

        [SerializeField] private PileView HandPile;
        [SerializeField] private PileView DeckPile;
        [SerializeField] private PileView DiscardPile;
        [SerializeField] private List<PileView> FriendlySlots;
        [SerializeField] private List<PileView> EnemySlots;

        private Context Context => SummerJam1Context.Instance.Context;
        private SummerJam1Events Events => SummerJam1Context.Instance.Events;
        private SummerJam1Game Game => SummerJam1Context.Instance.Game;

        private List<IDisposable> Disposables { get; } = new List<IDisposable>();

        protected void Awake()
        {
            Disposables.Add(Events.SubscribeToBattleEnded(OnBattleEnded));
            Disposables.Add(Events.SubscribeToBattleStarted(OnBattleStarted));
            Context.SetPrefabsDirectory(Path.Combine("Assets", "External", "Library", "Prefabs"));

            IEntity game = Context.Root;
            Events.SubscribeToUnitCreated(OnUnitCreated);
            Events.SubscribeToCardCreated(OnCardCreated);

            Game.StartBattle();
        }

        private void OnDestroy()
        {
            foreach (IDisposable disposable in Disposables)
            {
                disposable.Dispose();
            }
        }

        private void OnBattleStarted(object sender, BattleStartedEventArgs item)
        {
            //TODO: try getting rid of these bridge components by just listening to new events.
            CreateView(Game.Player.Entity, UnitPrefab);

            HandPile.SetModel(Game.Battle.Entity.GetComponentInChildren<HandPile>().Entity);
            HandPile.Entity.GetOrAddComponent<PileViewBridge>().gameObject = HandPile.gameObject;

            DeckPile.SetModel(Game.Battle.Entity.GetComponentInChildren<DeckPile>().Entity);
            DeckPile.Entity.GetOrAddComponent<PileViewBridge>().gameObject = DeckPile.gameObject;

            DiscardPile.SetModel(Game.Battle.Entity.GetComponentInChildren<PlayerDiscard>().Entity);
            DiscardPile.Entity.GetOrAddComponent<PileViewBridge>().gameObject = DiscardPile.gameObject;

            List<FriendlyUnitSlot> friendlySlotsModels = Game.Battle.Entity.GetComponentsInChildren<FriendlyUnitSlot>();
            int index = 0;
            foreach (PileView friendlySlot in FriendlySlots)
            {
                friendlySlot.SetModel(friendlySlotsModels[index].Entity);
                friendlySlot.Entity.AddComponent<PileViewBridge>().gameObject = friendlySlot.gameObject;
                index++;
            }

            List<EnemyUnitSlot> enemySlotsModels = Game.Entity.GetComponentsInChildren<EnemyUnitSlot>();
            index = 0;
            foreach (PileView enemySlot in EnemySlots)
            {
                enemySlot.SetModel(enemySlotsModels[index].Entity);
                enemySlot.Entity.AddComponent<PileViewBridge>().gameObject = enemySlot.gameObject;
                index++;
            }
        }

        private void OnBattleEnded(object sender, BattleEndedEventArgs item)
        {
            if (item.Victory)
            {
                Debug.Log("VICTOLY!");
            }
            else
            {
                Debug.Log("FAILURE.");
            }

            AnimationQueue.Instance.Enqueue(ReturnToMenu);
        }

        private IEnumerator ReturnToMenu()
        {
            yield return null;
            SceneManager.LoadScene("Scenes/SummerJam1/MenuScene");
        }

        private void OnCardCreated(object sender, CardCreatedEventArgs args)
        {
            var entity = args.CardId;
            CreateView(entity, SummerJam1CardFactory.Instance.CardPrefab);
        }

        private void OnUnitCreated(object sender, UnitCreatedEventArgs args)
        {
            var entity = args.Entity;
            CreateView(entity, UnitPrefab);
        }

        private void CreateView(IEntity entity, GameObject prefab)
        {
            GameObject unitView = Instantiate(prefab);
            unitView.transform.localPosition = Vector3.one * 10_000;
            unitView.GetComponent<ISetModel>().SetModel(entity);
            entity.GetOrAddComponent<SummerJam1ModelViewBridge>().gameObject = unitView;
        }


        public void EndTurn()
        {
            Game.EndTurn();
        }
    }
}