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
        public const int NUM_ENCOUNTER_SLOTS_PER_FLOOR = 5;
        public const int NUM_FLOORS = 4;
        public IEntity Discard { get; private set; }
        public IEntity Exhaust { get; private set; }
        public HandPile Hand { get; private set; }
        public DeckPile BattleDeck { get; private set; }
        public BeatTracker BeatTracker { get; private set; }

        public ObjectivesPile ObjectivesPile { get; private set; }

        public DeckPile EncounterDrawPile { get; private set; }
        public PlayerDiscard EncounterDiscardPile { get; private set; }

        public List<EncounterSlotPile> EncounterSlots { get; } = new();

        public bool BattleStarted { get; private set; }
        // public List<Pile> EncounterSlotsUpcoming { get; } = new();


        // public Dictionary<int, List<Pile>> AllEncounterSlots { get; } = new Dictionary<int, List<Pile>>();

        public int CurrentFloor { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();

            for (int j = 0; j < NUM_ENCOUNTER_SLOTS_PER_FLOOR; j++)
            {
                Context.CreateEntity(Entity, entity => EncounterSlots.Add(entity.AddComponent<EncounterSlotPile>()));
            }

            Context.CreateEntity(Entity, child => BeatTracker = child.AddComponent<BeatTracker>());
        }

        public void MoveToNextFloor()
        {
            CurrentFloor++;
        }

        public List<IEntity> GetTopEntitiesInAdjacentSlots(IEntity slotOrMonster)
        {
            List<IEntity> adjacents = new(2);
            EncounterSlotPile slot = slotOrMonster.GetComponentInSelfOrParent<EncounterSlotPile>();
            int index = EncounterSlots.IndexOf(slot);

            if (slot == null)
            {
                throw new NullReferenceException(nameof(slot));
            }

            if (index > 0 && EncounterSlots[index - 1].Entity.Children.Count > 0)
            {
                adjacents.Add(EncounterSlots[index - 1].Entity.Children.Last());
            }

            if (index < NUM_ENCOUNTER_SLOTS_PER_FLOOR - 1 && EncounterSlots[index + 1].Entity.Children.Count > 0)
            {
                adjacents.Add(EncounterSlots[index + 1].Entity.Children.Last());
            }


            return adjacents;
        }


        public List<IEntity> GetTopEntitiesInAllSlots()
        {
            return GetNonEmptySlots().Select(pile => pile.Entity.Children.LastOrDefault()).ToList();
        }

        public bool IsTopCard(IEntity card)
        {
            return card.Parent.Children.LastOrDefault() == card;
        }

        public IEntity GetTopCard(IEntity slot)
        {
            return slot.Children.LastOrDefault();
        }

        public IEntity GetSlotToRight(IEntity slotOrMonster)
        {
            EncounterSlotPile slot = slotOrMonster.GetComponentInSelfOrParent<EncounterSlotPile>();
            int index = EncounterSlots.IndexOf(slot);
            if (slot == null)
            {
                throw new NullReferenceException(nameof(slot));
            }


            if (index < EncounterSlots.Count - 1)
            {
                return EncounterSlots[index + 1].Entity;
            }

            return null;
        }

        public IEntity GetSlotToLeft(IEntity slotOrMonster)
        {
            EncounterSlotPile slot = slotOrMonster.GetComponentInSelfOrParent<EncounterSlotPile>();
            int index = EncounterSlots.IndexOf(slot);
            if (slot == null)
            {
                throw new NullReferenceException(nameof(slot));
            }


            if (index >= 1)
            {
                return EncounterSlots[index - 1].Entity;
            }

            return null;
        }

        public List<IEntity> GetAdjacentSlots(IEntity slotOrMonster)
        {
            List<IEntity> adjacents = new(2);
            EncounterSlotPile slot = slotOrMonster.GetComponentInSelfOrParent<EncounterSlotPile>();
            int index = EncounterSlots.IndexOf(slot);
            if (slot == null)
            {
                throw new NullReferenceException(nameof(slot));
            }

            if (index > 0)
            {
                adjacents.Add(EncounterSlots[index - 1].Entity);
            }

            if (index < EncounterSlots.Count - 1)
            {
                adjacents.Add(EncounterSlots[index + 1].Entity);
            }


            return adjacents;
        }

        public IEnumerable<Pile> GetEmptySlots()
        {
            return EncounterSlots.Where(slot => slot.Entity.Children.Count == 0);
        }

        public IEnumerable<Pile> GetNonEmptySlots()
        {
            return EncounterSlots.Where(slot => slot.Entity.Children.Count > 0);
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

        public void StartBattle(DungeonPile pile)
        {
            pile.Entity.TrySetParent(Entity);

            SetupBattleDeck();

            Context.CreateEntity(Entity, entity => ObjectivesPile = entity.AddComponent<ObjectivesPile>());
            Context.CreateEntity(Entity, entity => EncounterDrawPile = entity.AddComponent<DeckPile>());
            Context.CreateEntity(Entity, entity => EncounterDiscardPile = entity.AddComponent<PlayerDiscard>());


            PopulateEncounterPiles(pile);

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

        public void EndDungeonPhase()
        {
            // Events.OnDungeonPhaseEnded(new DungeonPhaseEndedEventArgs());
            Events.OnDrawPhaseBegan(new DrawPhaseBeganEventArgs());
            Events.OnTurnBegan(new TurnBeganEventArgs());
        }

        public bool TryMoveUnitRight(IEntity unit)
        {
            IEntity targetSlot = Game.Battle.GetSlotToRight(unit);
            if (targetSlot == null)
            {
                return false;
            }

            return TryMoveUnit(unit, targetSlot);
        }

        public bool TryMoveUnitLeft(IEntity unit)
        {
            IEntity targetSlot = Game.Battle.GetSlotToLeft(unit);
            if (targetSlot == null)
            {
                return false;
            }

            return TryMoveUnit(unit, targetSlot);
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

        private void PopulateEncounterPiles(DungeonPile pile)
        {
            Logging.Log($"Starting battle with {pile.Entity.Children.Count} encounters.");
            int index = 0;
            List<IEntity>[] lists = new List<IEntity>[NUM_ENCOUNTER_SLOTS_PER_FLOOR];
            for (int i = 0; i < lists.Length; i++)
            {
                lists[i] = new List<IEntity>();
            }

            foreach (IEntity entity in pile.Entity.Children.OrderByDescending(entity => entity.HasComponent<IBottomCard>())
                         .ThenBy(DungeonOrder)) //fill encounter slots.
            {
                int i = index % EncounterSlots.Count;

                lists[i].Add(entity);


                index += 2;
            }

            for (int i = 0; i < lists.Length; i++)
            {
                //lists[i] = lists[i].OrderByDescending(entity => entity.GetComponent<Health>()?.Amount).ToList();

                foreach (IEntity entity in lists[i])
                {
                    if (!entity.TrySetParent(EncounterSlots[i].Entity))
                    {
                        Logging.LogError($"Failed to parent encounter card! : {entity.GetComponent<NameComponent>().Value}.");
                    }
                }
            }

            foreach (EncounterSlotPile encounterSlot in EncounterSlots)
            {
                encounterSlot.SetAllButTopFaceDown();
            }
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
