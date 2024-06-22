using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using SummerJam1.Piles;
using SummerJam1.Units;
using Random = Api.Random;

namespace SummerJam1
{
    public class BattleContainer : SummerJam1Component
    {
        public const int NUM_FLOORS = 4;
        public IEntity Discard { get; private set; }
        public IEntity Exhaust { get; private set; }
        public HandPile Hand { get; private set; }
        public DeckPile BattleDeck { get; private set; }

        public ObjectivesPile ObjectivesPile { get; private set; }

        public EncounterSlotPile EncounterSlots { get; set; }

        public bool BattleStarted { get; private set; }

        public int CurrentFloor { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();

            Context.CreateEntity(Entity, entity => EncounterSlots = entity.AddComponent<EncounterSlotPile>());
        }

        public void MoveToNextFloor()
        {
            CurrentFloor++;
        }

        public bool IsTopCard(IEntity card)
        {
            return card.Parent.Children.LastOrDefault() == card;
        }

        public IEntity GetTopCard(IEntity slot)
        {
            return slot.Children.LastOrDefault();
        }


        public IEntity CreateRandomMonster(IEntity parent, int difficultyMin, int difficultyMax)
        {
            string name = GetRandomMonsterPrefab(difficultyMin, difficultyMax, Entity.GetComponentInParent<Random>());
            return Context.CreateEntity(parent, name);
        }

        public static string GetRandomMonsterPrefab(int difficultyMin, int difficultyMax, Random random)
        {
            int difficulty = random.SystemRandom.Next(difficultyMin, difficultyMax + 1);


            DirectoryInfo info = new(Path.Combine(Context.PrefabsPath, "Units", "Standard", $"{difficulty}"));
            List<FileInfo> files = info.GetFiles().Where(file => file.Extension == ".json").ToList();

            int index = random.SystemRandom.Next(files.Count);
            string name = Path.Combine("Units", "Standard", $"{difficulty}", files[index].Name);
            return name;
        }

        public void StartBattle()
        {
           
            SetupBattleDeck();

            Context.CreateEntity(Entity, entity => ObjectivesPile = entity.AddComponent<ObjectivesPile>());



            Exhaust = Context.CreateEntity(Entity, entity =>
            {
                entity.AddComponent<PlayerExhaust>();
                entity.AddComponent<PlayerControl>();
            });

            BattleStarted = true;
            Events.OnBattleStarted(new BattleStartedEventArgs());
            Events.OnDrawPhaseBegan(new DrawPhaseBeganEventArgs());
            Events.OnTurnBegan(new TurnBeganEventArgs());
        }

        private void SetupBattleDeck()
        {
            BattleDeck = Context.DuplicateEntity(Game.Deck.Entity).GetComponent<DeckPile>();
            BattleDeck.Entity.AddComponent<PlayerControl>();

            BattleDeck.Entity.TrySetParent(Entity);
            Context.CreateEntity(Entity, entity =>
            {
                Hand = entity.AddComponent<HandPile>();
                entity.AddComponent<PlayerControl>();
            });
            Discard = Context.CreateEntity(Entity, entity =>
            {
                entity.AddComponent<PlayerDiscard>();
                entity.AddComponent<PlayerControl>();
            });

            BattleDeck.SetHandAndDiscard(Hand.Entity, Discard);
        }


        public bool TryMoveUnit(IEntity unit, IEntity targetSlot)
        {
            RequestMoveUnitEventArgs tryMoveArgs = new(Entity, false, targetSlot);
            if (tryMoveArgs.Blockers.Any())
            {
                foreach (string blocker in tryMoveArgs.Blockers)
                {
                    Logging.Log($"Unable to push card : {blocker}");
                }

                return false;
            }

            if (unit.TrySetParent(targetSlot))
            {
                Events.OnUnitMoved(new UnitMovedEventArgs(unit, false, targetSlot));
                return true;
            }

            return false;
        }
        

        private int DungeonOrder(IEntity _)
        {
            return Game.Random.SystemRandom.Next();
        }


        private List<IEntity> CreateRandomObjectives(int count = 2)
        {
            DirectoryInfo info = new(Path.Combine(Context.PrefabsPath, "Objectives"));
            List<FileInfo> files = info.GetFiles().Where(file => file.Extension == ".json").ToList();
            if (count > files.Count)
            {
                throw new ArgumentException(nameof(count));
            }

            List<int> indices = new(count);

            while (indices.Count < count)
            {
                int index = Game.Random.SystemRandom.Next(files.Count);
                if (!indices.Contains(index))
                {
                    indices.Add(index);
                }
            }

            List<IEntity> objectives = new(count);
            foreach (int index in indices)
            {
                objectives.Add(Context.CreateEntity(null, Path.Combine("Objectives", files[index].Name)));
            }

            return objectives;
        }

        public List<IEntity> GetAllPlayerCardsInPlay()
        {
            //discard hand and battle deck.
            return Discard.Children.Concat(Hand.Entity.Children).Concat(BattleDeck.Entity.Children).ToList();
        }
    }
}