using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Api;
using App;
using CardsAndPiles;
using CardsAndPiles.Components;
using External.UnityAsync.UnityAsync.Assets.UnityAsync;
using External.UnityAsync.UnityAsync.Assets.UnityAsync.Await;
using Guirao.UltimateTextDamage;
using SummerJam1.Units;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SummerJam1
{
    public class SummerJam1Helper : MonoBehaviour //essentially a view<Battle>
    {
        [SerializeField] private UltimateTextDamageManager m_TextDamageManager;

        [SerializeField] private PileView HandPile;
        [SerializeField] private PileView DeckPile;
        [SerializeField] private PileView ExhaustPile;
        [SerializeField] private PileView DiscardPile;
        [SerializeField] private List<PileView> FriendlySlots;

        [SerializeField] private List<PileView> EnemySlots;

        // [SerializeField] private GameObject PlayerView;
        [SerializeField] private GameObject VictoryPopup;


        private Context Context => SummerJam1Context.Instance.Context;
        private SummerJam1Events Events => SummerJam1Context.Instance.Events;
        private Game Game => SummerJam1Context.Instance.Game;

        private List<IDisposable> Disposables { get; } = new List<IDisposable>();

        protected void Awake()
        {
            Disposables.Add(Events.SubscribeToBattleEnded(OnBattleEnded));
            Disposables.Add(Events.SubscribeToBattleStarted(OnBattleStarted));



            IEntity game = Context.Root;
            Disposables.Add(Events.SubscribeToUnitCreated(OnUnitCreated));
            Disposables.Add(Events.SubscribeToDamageDealt(OnDamageDealt));
            Disposables.Add(Events.SubscribeToHealDealt(OnHealDealt));

            Game.StartBattle();
        }

        private void OnHealDealt(object sender, HealDealtEventArgs item)
        {
            var targetGo = item.EntityId.GetComponent<IGameObject>();
            if (targetGo != null)
            {
                QueueText(targetGo.gameObject.transform, item.Amount.ToString(), "healing");
            }
        }

        private void OnDamageDealt(object sender, DamageDealtEventArgs item)
        {
            var targetGo = item.EntityId.GetComponent<IGameObject>();
            if (targetGo != null)
            {
                QueueText(targetGo.gameObject.transform, item.Amount.ToString(), "damage");
            }
        }

        private void QueueText(Transform target, string text, string key)
        {
            AnimationQueue.Instance.Enqueue(async () =>
            {
                await new WaitForEndOfFrame();
                m_TextDamageManager.Add(text, target, key);
            });
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
            //CreateView(Game.Player.Entity, UnitPrefab);
            // PlayerView.GetComponent<ISetModel>().SetModel(Game.Player.Entity);


            HandPile.SetModel(Game.Battle.Entity.GetComponentInChildren<HandPile>().Entity);
            HandPile.Entity.GetOrAddComponent<PileViewBridge>().gameObject = HandPile.gameObject;

            DeckPile.SetModel(Game.Battle.BattleDeck.Entity);
            DeckPile.Entity.GetOrAddComponent<PileViewBridge>().gameObject = DeckPile.gameObject;

            ExhaustPile.SetModel(Game.Battle.Exhaust);
            ExhaustPile.Entity.GetOrAddComponent<PileViewBridge>().gameObject = ExhaustPile.gameObject;

            DiscardPile.SetModel(Game.Battle.Entity.GetComponentInChildren<PlayerDiscard>().Entity);
            DiscardPile.Entity.GetOrAddComponent<PileViewBridge>().gameObject = DiscardPile.gameObject;

            List<FriendlyUnitSlot> friendlySlotsModels = Game.Battle.Entity.GetComponentsInChildren<FriendlyUnitSlot>();
            int index = 0;
            foreach (FriendlyUnitSlot friendlySlot in friendlySlotsModels.OrderBy(slot => slot.Order))
            {
                FriendlySlots[index].SetModel(friendlySlot.Entity);
                friendlySlot.Entity.AddComponent<PileViewBridge>().gameObject = FriendlySlots[index].gameObject;
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

            Debug.Log($"{Game.Entity.GetComponentsInChildren<Card>().Count} cards found in game.");
        }

        private void OnBattleEnded(object sender, BattleEndedEventArgs item)
        {
            if (item.Victory)
            {
                Debug.Log("VICTOLY!");
                Disposables.Add(AnimationQueue.Instance.Enqueue(ReturnToMenu));
            }
            else
            {
                Debug.Log("FAILURE.");
            }
        }

        private void Update()
        {
        }

        private async void ReturnToMenu()
        {
            VictoryPopup.SetActive(true);
            while (VictoryPopup.activeInHierarchy)
            {
                await Await.NextUpdate();
            }

            SceneManager.LoadScene("Scenes/SummerJam1/MenuScene");
        }


        private void OnUnitCreated(object sender, UnitCreatedEventArgs args)
        {
            var entity = args.Entity;
            SummerJam1Context.CreateView(entity, SummerJam1UnitFactory.Instance.UnitPrefab);
        }


        public void EndTurn()
        {
            Game.EndTurn();
        }
    }
}
