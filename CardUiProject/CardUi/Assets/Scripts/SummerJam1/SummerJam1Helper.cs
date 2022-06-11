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


        private Context Context { get; set; }
        private SummerJam1Events Events => (SummerJam1Events)Context.Events;
        private SummerJam1Game Game { get; set; }

        protected void Awake()
        {
            var events = new SummerJam1Events();
            Context = new Context(events);
            Events.SubscribeToBattleEnded(OnBattleEnded);
            Events.SubscribeToBattleStarted(OnBattleStarted);
            Context.SetPrefabsDirectory(Path.Combine("Assets", "External", "Library", "Prefabs"));

            IEntity game = Context.Root;
            events.SubscribeToUnitCreated(OnUnitCreated);

            Game = game.AddComponent<SummerJam1Game>();


            Game.StartBattle();
        }

        private void OnBattleStarted(object sender, BattleStartedEventArgs item)
        {
            //TODO: try getting rid of these bridge components by just listening to new events.
            item.Battle.Entity.AddComponent<SummerJam1CardViewBridge>();
            item.Battle.Entity.AddComponent<SummerJam1UnitViewBridge>();
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

        private void OnUnitCreated(object sender, UnitCreatedEventArgs args)
        {
            GameObject unitView = Instantiate(UnitPrefab);
            unitView.transform.localPosition = Vector3.one * 10_000;
            unitView.GetComponent<IView>().SetModel(args.Entity);
            args.Entity.GetOrAddComponent<SummerJam1UnitViewBridge>().gameObject = unitView;
        }


        public void EndTurn()
        {
            Game.EndTurn();
        }
    }
}