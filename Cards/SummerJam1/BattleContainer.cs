using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Api;
using CardsAndPiles;
using SummerJam1.Cards;

namespace SummerJam1
{
    public class BattleContainer : SummerJam1Component
    {
        public IEntity Discard { get; private set; }
        public IEntity Exhaust { get; private set; }
        public HandPile Hand { get; private set; }
        public DeckPile BattleDeck { get; private set; }

        public ObjectivesPile ObjectivesPile { get; private set; }

        public DeckPile EncounterDrawPile { get; private set; }
        public HandPile EncounterHandPile { get; private set; }
        public PlayerDiscard EncounterDiscardPile { get; private set; }
        public List<Pile> EncounterSlots { get; private set; } = new List<Pile>();

        public const int NumEncounterSlots = 5;

        public List<IEntity> GetEntitiesInAdjacentSlots(IEntity slotOrMonster)
        {
            List<IEntity> adjacents = new List<IEntity>(2);
            var slot = slotOrMonster.GetComponentInSelfOrParent<EncounterSlotPile>();
            var index = EncounterSlots.IndexOf(slot);

            if (slot == null)
            {
                throw new NullReferenceException(nameof(slot));
            }

            if (index > 0 && EncounterSlots[index - 1].Entity.Children.Count > 0)
            {
                adjacents.Add(EncounterSlots[index - 1].Entity.Children.First());
            }

            if (index < NumEncounterSlots - 1 && EncounterSlots[index + 1].Entity.Children.Count > 0)
            {
                adjacents.Add(EncounterSlots[index + 1].Entity.Children.First());
            }


            return adjacents;
        }

        public List<IEntity> GetAdjacentSlots(IEntity slotOrMonster)
        {
            List<IEntity> adjacents = new List<IEntity>(2);
            var slot = slotOrMonster.GetComponentInSelfOrParent<EncounterSlotPile>();
            var index = EncounterSlots.IndexOf(slot);
            if (slot == null)
            {
                throw new NullReferenceException(nameof(slot));
            }

            if (index > 0 && EncounterSlots[index - 1].Entity.Children.Count > 0)
            {
                adjacents.Add(EncounterSlots[index - 1].Entity);
            }

            if (index < NumEncounterSlots - 1 && EncounterSlots[index + 1].Entity.Children.Count > 0)
            {
                adjacents.Add(EncounterSlots[index + 1].Entity);
            }


            return adjacents;
        }

        public IEntity CreateRandomMonster(IEntity parent, int difficulty)
        {
            DirectoryInfo info = new DirectoryInfo(Path.Combine(Context.PrefabsPath, $"Units", "Standard", $"{difficulty}"));
            List<FileInfo> files = info.GetFiles().Where(file => file.Extension == ".json").ToList();

            int index = Game.Random.SystemRandom.Next(files.Count);

            return Context.CreateEntity(parent, Path.Combine($"Units", $"{difficulty}", files[index].Name));
        }

        public void StartBattle()
        {
            BattleDeck = Context.DuplicateEntity(Game.Deck.Entity).GetComponent<DeckPile>();
            BattleDeck.Entity.TrySetParent(Entity);
            Context.CreateEntity(Entity, entity =>
                Hand = entity.AddComponent<HandPile>());
            Discard = Context.CreateEntity(Entity, entity => entity.AddComponent<PlayerDiscard>());

            BattleDeck.SetHandAndDiscard(Hand.Entity, Discard);

            Context.CreateEntity(Entity, (entity) => ObjectivesPile = entity.AddComponent<ObjectivesPile>());


            Context.CreateEntity(Entity, entity => EncounterDrawPile = entity.AddComponent<DeckPile>());
            Context.CreateEntity(Entity, entity => EncounterHandPile = entity.AddComponent<HandPile>());
            Context.CreateEntity(Entity, entity => EncounterDiscardPile = entity.AddComponent<PlayerDiscard>());
            EncounterDrawPile.SetHandAndDiscard(EncounterHandPile.Entity, EncounterDiscardPile.Entity);

            for (int i = 0; i < NumEncounterSlots; i++)
            {
                Context.CreateEntity(Entity, entity => EncounterSlots.Add(entity.AddComponent<EncounterSlotPile>()));
            }

            PopulateEncounterDrawPile();

            Exhaust = Context.CreateEntity(Entity, entity => entity.AddComponent<PlayerExhaust>());

            Events.OnBattleStarted(new BattleStartedEventArgs());
            Events.OnDungeonPhaseStarted(new DungeonPhaseStartedEventArgs());
        }

        private void PopulateEncounterDrawPile()
        {
            for (int i = 0; i < 3; i++)
            {
                // Context.CreateEntity(EncounterDrawPile.Entity, "Units/mcguffin.json");
                // Context.CreateEntity(EncounterDrawPile.Entity, "Units/treasureChest.json");
                Context.CreateEntity(EncounterDrawPile.Entity, "Units/standard/1/notGengar.json");
                Context.CreateEntity(EncounterDrawPile.Entity, "Units/standard/1/birthdayBoy.json");
                Context.CreateEntity(EncounterDrawPile.Entity, "Units/standard/1/sadRalph.json");
            }

            Context.CreateEntity(EncounterDrawPile.Entity, "Units/boss.json");
        }


        private List<IEntity> CreateRandomObjectives(int count = 2)
        {
            DirectoryInfo info = new DirectoryInfo(Path.Combine(Context.PrefabsPath, "Objectives"));
            List<FileInfo> files = info.GetFiles().Where(file => file.Extension == ".json").ToList();
            if (count > files.Count)
            {
                throw new ArgumentException(nameof(count));
            }

            List<int> indices = new List<int>(count);

            while (indices.Count < count)
            {
                int index = Game.Random.SystemRandom.Next(files.Count);
                if (!indices.Contains(index))
                {
                    indices.Add(index);
                }
            }

            List<IEntity> objectives = new List<IEntity>(count);
            foreach (int index in indices)
            {
                objectives.Add(Context.CreateEntity(null, Path.Combine("Objectives", files[index].Name)));
            }

            return objectives;
        }
    }
}
