using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Cards;
using SummerJam1.Rules;
using SummerJam1.Units;

namespace SummerJam1
{
    public class SummerJam1Game : SummerJam1Component
    {
        public DeckPile Deck { get; private set; }

        public BattleContainer Battle { get; private set; }
        public Player Player { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();
            AddRules();
            Context.CreateEntity(Entity, entity =>
            {
                Player = entity.AddComponent<Player>();
                entity.AddComponent<PlayerUnit>();
                entity.AddComponent<VisualComponent>().AssetName = SummerJam1UnitAsset.Player;
                Health health = entity.AddComponent<Health>();
                health.SetMax(100);
                health.SetHealth(100);
            });

            //create an example deck.
            Context.CreateEntity(Entity, entity => Deck = entity.AddComponent<DeckPile>());
            for (int i = 0; i < 20; i++)
            {
                Context.CreateEntity(Deck.Entity, entity =>
                {
                    entity.AddComponent<StarterUnitCard>();
                    entity.AddComponent<NameComponent>().Value = "Pepper";
                });
                Context.CreateEntity(Deck.Entity, "Dice.json");
                Context.CreateEntity(Deck.Entity, "DrawNewHand.json");
            }
        }

        private void AddRules()
        {
            Context.Root.AddComponent<DiscardHandOnTurnEnd>();
            Context.Root.AddComponent<DrawHandOnTurnBegin>();
        }

        public void EndTurn()
        {
            Events.OnTurnEnded(new TurnEndedEventArgs());
            Events.OnTurnBegan(new TurnBeganEventArgs());
        }


        public void StartBattle()
        {
            if (Battle != null)
            {
                Battle.Entity.Destroy();
            }

            Context.CreateEntity(Entity, (entity) => { Battle = entity.AddComponent<BattleContainer>(); });
            Player.Entity.TrySetParent(Battle.Entity.GetComponentInChildren<FriendlyUnitSlot>().Entity);
            Battle.StartBattle();
        }

        [OnBattleEnded]
        private void OnBattleEnded(object sender, BattleEndedEventArgs args)
        {
            if (args.Victory)
            {
                Player.Entity.TrySetParent(Entity);
            }
        }
    }

    public class Player : SummerJam1Component
    {
        public int CurrentEnergy { get; private set; }
        public int MaxEnergy { get; private set; } = 3;

        public bool TryUseEnergy(int amount)
        {
            if (CurrentEnergy < amount)
            {
                return false;
            }

            CurrentEnergy -= amount;
            //invoke energy used.

            return true;
        }


        [OnTurnBegan]
        private void OnTurnBegan()
        {
            CurrentEnergy = MaxEnergy;
        }
    }
}