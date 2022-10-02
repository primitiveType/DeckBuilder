using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Api;
using CardsAndPiles;
using SummerJam1.Piles;
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

        public ObjectivesPile ObjectivesPile { get; private set; }

        public DeckPile EncounterDrawPile { get; private set; }
        public PlayerDiscard EncounterDiscardPile { get; private set; }

        public List<Pile> EncounterSlots { get; } = new();
        // public List<Pile> EncounterSlotsUpcoming { get; } = new();


        // public Dictionary<int, List<Pile>> AllEncounterSlots { get; } = new Dictionary<int, List<Pile>>();

        public int CurrentFloor { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();
            // for (int i = 0; i < NumFloors; i++)
            // {
            //     AllEncounterSlots[i] = new List<Pile>();
            //     for (int j = 0; j < NumEncounterSlotsPerFloor; j++)
            //     {
            //         int index = i;
            //         Context.CreateEntity(Entity, entity => AllEncounterSlots[index].Add(entity.AddComponent<EncounterSlotPile>()));
            //     }
            // }

            for (int j = 0; j < NUM_ENCOUNTER_SLOTS_PER_FLOOR; j++)
            {
                Context.CreateEntity(Entity, entity => EncounterSlots.Add(entity.AddComponent<EncounterSlotPile>()));
                // Context.CreateEntity(Entity, entity => EncounterSlotsUpcoming.Add(entity.AddComponent<UpcomingEncounterSlotPile>()));
            }
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
            return GetFullSlots().Select(pile => pile.Entity.Children.LastOrDefault()).ToList();
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

        public IEnumerable<Pile> GetFullSlots()
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
            List<PrefabReference> componentsInChildren = pile.Entity.GetComponentsInChildren<PrefabReference>();
            List<string> prefabs = componentsInChildren.Select(component => component.Prefab).ToList();

            pile.Entity.TrySetParent(Entity);

            SetupBattleDeck();

            Context.CreateEntity(Entity, entity => ObjectivesPile = entity.AddComponent<ObjectivesPile>());


            Context.CreateEntity(Entity, entity => { EncounterDrawPile = entity.AddComponent<DeckPile>(); });

            Context.CreateEntity(Entity, entity => { EncounterDiscardPile = entity.AddComponent<PlayerDiscard>(); });


            PopulateEncounterPiles(prefabs);

            Exhaust = Context.CreateEntity(Entity, entity =>
            {
                entity.AddComponent<PlayerExhaust>();
                entity.AddComponent<PlayerControl>();
            });

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

        private void PopulateEncounterPiles(List<string> prefabs)
        {
            Logging.Log($"Starting battle with {prefabs.Count} encounters.");
            int index = 0;
            foreach (string prefab in prefabs) //fill encounter slots.
            {
                int i = index % EncounterSlots.Count;
                Context.CreateEntity(EncounterSlots[i].Entity, prefab);
                index++;
            }
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
