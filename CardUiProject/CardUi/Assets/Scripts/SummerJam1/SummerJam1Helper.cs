using System.Collections.Generic;
using Api;
using CardsAndPiles;
using Common;
using SummerJam1.Units;
using UnityEngine;
using UnityEngine.XR;

namespace SummerJam1
{
    public class SummerJam1Helper : MonoBehaviourSingleton<SummerJam1Helper>
    {
        [SerializeField] private GameObject m_CardPrefab;
        [SerializeField] private GameObject m_UnitPrefab;
        public GameObject CardPrefab => m_CardPrefab;
        public GameObject UnitPrefab => m_UnitPrefab;

        [SerializeField] private PileView HandPile;
        [SerializeField] private PileView DeckPile;
        [SerializeField] private PileView DiscardPile;
        [SerializeField] private List<PileView> FriendlySlots;
        [SerializeField] private List<PileView> EnemySlots;

        private Context Context { get; set; }
        private SummerJam1Events Events => (SummerJam1Events)Context.Events;
        private SummerJam1Game Game { get; set; }

        protected override void Awake()
        {
            base.Awake();
            var events = new SummerJam1Events();
            Context = new Context(events);
            IEntity game = Context.Root;
            events.SubscribeToUnitCreated(OnUnitCreated);

            Game = game.AddComponent<SummerJam1Game>();
            


            game.AddComponent<SummerJam1CardViewBridge>();
            game.AddComponent<SummerJam1UnitViewBridge>();

            HandPile.SetModel(game.GetComponentInChildren<HandPile>().Entity);
            HandPile.Entity.AddComponent<PileViewBridge>().gameObject = HandPile.gameObject;

            DeckPile.SetModel(game.GetComponentInChildren<DeckPile>().Entity);
            DeckPile.Entity.AddComponent<PileViewBridge>().gameObject = DeckPile.gameObject;

            DiscardPile.SetModel(game.GetComponentInChildren<PlayerDiscard>().Entity);
            DiscardPile.Entity.AddComponent<PileViewBridge>().gameObject = DiscardPile.gameObject;

            List<FriendlyUnitSlot> friendlySlotsModels = game.GetComponentsInChildren<FriendlyUnitSlot>();
            int index = 0;
            foreach (PileView friendlySlot in FriendlySlots)
            {
                friendlySlot.SetModel(friendlySlotsModels[index].Entity);
                friendlySlot.Entity.AddComponent<PileViewBridge>().gameObject = friendlySlot.gameObject;
                index++;
            }
            
            List<EnemyUnitSlot> enemySlotsModels = game.GetComponentsInChildren<EnemyUnitSlot>();
            index = 0;
            foreach (PileView enemySlot in EnemySlots)
            {
                enemySlot.SetModel(enemySlotsModels[index].Entity);
                enemySlot.Entity.AddComponent<PileViewBridge>().gameObject = enemySlot.gameObject;
                index++;
            }


            Game.StartBattle();
        }


        private void OnUnitCreated(object sender, UnitCreatedEventArgs args)
        {
            GameObject unitView = Instantiate(UnitPrefab);
            unitView.GetComponent<IView>().SetModel(args.Entity);
            args.Entity.AddComponent<SummerJam1UnitViewBridge>().gameObject = unitView;
        }

        public void EndTurn()
        {
            Game.EndTurn();
        }
    }
}