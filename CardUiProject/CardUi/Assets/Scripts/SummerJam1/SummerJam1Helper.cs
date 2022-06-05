using System.Collections.Generic;
using Api;
using CardsAndPiles;
using Common;
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

        protected override void Awake()
        {
            base.Awake();
            var events = new SummerJam1Events();

            Context context = new Context(events);

            IEntity game = context.Root;
            game.AddComponent<SummerJam1Game>();

            events.SubscribeToUnitCreated(OnUnitCreated);
            events.SubscribeToCardDiscarded(OnCardDiscarded);
            events.SubscribeToCardPlayed(OnCardPlayed);


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
        }

        private void OnCardPlayed(object sender, CardPlayedEventArgs args)
        {
            Debug.Log("Card played.");
        }

        private void OnCardDiscarded(object sender, CardDiscardedEventArgs args)
        {
            Debug.Log("Card discarded.");
        }

        private void OnUnitCreated(object sender, UnitCreatedEventArgs args)
        {
            Debug.Log("Unit created.");

            GameObject unitView = Instantiate(UnitPrefab);
            args.EntityId.AddComponent<SummerJam1UnitViewBridge>().gameObject = unitView;
        }
    }
}