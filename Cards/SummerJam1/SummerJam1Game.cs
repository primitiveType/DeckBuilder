using System.Collections.Generic;
using System.IO;
using System.Linq;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Rules;
using SummerJam1.Units;

namespace SummerJam1
{
    public class SummerJam1Game : SummerJam1Component
    {
        public DeckPile Deck { get; private set; }
        public SummerJam1PrizePile PrizePile { get; private set; }

        public BattleContainer Battle { get; private set; }
        public Player Player { get; private set; }

        private UnitSlot PlayerSlot { get; set; }

        private Random Random { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            AddRules();
            Random = Entity.AddComponent<Random>();
            Context.CreateEntity(Entity, entity => PrizePile = entity.AddComponent<SummerJam1PrizePile>());
            Context.CreateEntity(Entity, entity => { PlayerSlot = entity.AddComponent<UnitSlot>(); });
            Context.CreateEntity(Entity, entity =>
            {
                Player = entity.AddComponent<Player>();
                entity.AddComponent<PlayerUnit>();
                entity.AddComponent<UnitVisualComponent>().AssetName = SummerJam1UnitAsset.Player;
                Health health = entity.AddComponent<Health>();
                health.SetMax(100);
                health.SetHealth(100);
            });

            //create an example deck.
            Context.CreateEntity(Entity, entity => Deck = entity.AddComponent<DeckPile>());
            for (int i = 0; i < 2; i++)
            {
                Context.CreateEntity(Deck.Entity, "Cards/Starter.json");
                Context.CreateEntity(Deck.Entity, "Cards/Starter.json");
                Context.CreateEntity(Deck.Entity, "Cards/Dice.json");
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
            //Player.Entity.TrySetParent(Battle.GetBackMostEmptySlot());
            Battle.StartBattle();
        }
        

        public IEntity CreateRandomCard()
        {
            DirectoryInfo info = new DirectoryInfo(Path.Combine(Context.PrefabsPath, "Cards"));
            List<FileInfo> files = info.GetFiles().Where(file=>file.Extension == ".json").ToList();
           
            int index = Random.SystemRandom.Next(files.Count);

            return Context.CreateEntity(null, Path.Combine("Cards", files[index].Name));
        }
        
        
    }
}