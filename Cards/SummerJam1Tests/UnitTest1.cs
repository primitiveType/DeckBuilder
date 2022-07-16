using System;
using System.IO;
using System.Linq;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using NUnit.Framework;
using NUnit.Framework.Internal;
using RogueMaps;
using SummerJam1;
using SummerJam1.Cards;
using SummerJam1.Units;
using ILogger = Api.ILogger;

namespace SummerJam1Tests
{
    public class TestLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }

        public void LogWarning(string message)
        {
            Console.WriteLine(message);
        }

        public void LogError(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class Tests
    {
        private Context Context { get; set; }
        private Game Game { get; set; }

        [SetUp]
        public void Setup()
        {
            Logging.Initialize(new TestLogger());
            Context = new Context(new SummerJam1Events());
            IEntity gameEntity = Context.Root;
            Context.SetPrefabsDirectory("StreamingAssets");

            Game = gameEntity.AddComponent<Game>();
        }

        [Test]
        public void TestSaveLoad()
        {
            string contextStr = Serializer.Serialize(Context);
            Context copy = Serializer.Deserialize<Context>(contextStr);

            Assert.AreEqual(copy.PrefabsPath, Context.PrefabsPath);
            Assert.That(copy.Root.GetComponent<Game>(), Is.Not.Null);
        }

        [Test]
        public void TestUnitCreatedInSlot()
        {
            UnitCard unitCard = MakeUnitCard();
            Game.StartBattle("Units/Donut.json");
            FriendlyUnitSlot unitSlot = Game.Entity.GetComponentsInChildren<FriendlyUnitSlot>().First(slot => slot.Entity.Children.Count == 0);

            Assert.NotNull(unitSlot);
            Assert.IsTrue(unitCard.TryPlayCard(unitSlot.Entity));

            Assert.NotNull(unitSlot.Entity.GetComponentInChildren<Unit>());

            UnitCard unitCard2 = MakeUnitCard();
            Assert.IsFalse(unitCard2.TryPlayCard(unitSlot.Entity));
        }

        [Test]
        public void StartBattle()
        {
            Game.StartBattle("Units/Donut.json");
        }

        [Test]
        public void AllCardsHaveNameAndDescription()
        {
            DirectoryInfo info = new DirectoryInfo(Path.Combine(Context.PrefabsPath, "Cards"));

            TestDirectory(info, "Cards");

            void TestDirectory(DirectoryInfo dir, string relativePath)
            {
                foreach (DirectoryInfo enumerateDirectory in dir.EnumerateDirectories())
                {
                    TestDirectory(enumerateDirectory, Path.Combine(relativePath, enumerateDirectory.Name));
                }

                foreach (FileInfo enumerateFile in dir.EnumerateFiles())
                {
                    IEntity entity = Context.CreateEntity(null, Path.Combine(relativePath, enumerateFile.Name));
                    Assert.NotNull(entity);
                    Assert.That(entity.Components, Has.Count.GreaterThan(0));
                    Assert.NotNull(entity.GetComponent<IDescription>(), enumerateFile.Name);
                    Assert.NotNull(entity.GetComponent<NameComponent>(), enumerateFile.Name);
                }
            }
        }

        [Test]
        public void TryLoadAllPrefabs()
        {
            DirectoryInfo info = new DirectoryInfo(Context.PrefabsPath);

            TestDirectory(info, "");

            void TestDirectory(DirectoryInfo dir, string relativePath)
            {
                foreach (DirectoryInfo enumerateDirectory in dir.EnumerateDirectories())
                {
                    TestDirectory(enumerateDirectory, Path.Combine(relativePath, enumerateDirectory.Name));
                }

                foreach (FileInfo enumerateFile in dir.EnumerateFiles())
                {
                    IEntity entity = Context.CreateEntity(null, Path.Combine(relativePath, enumerateFile.Name));
                    Assert.NotNull(entity);
                    Assert.That(entity.Components, Has.Count.GreaterThan(0));
                }
            }
        }

        // [Test]
        // public void Serialize_Card()
        // {
        //     var card = Context.CreateEntity();
        //     card.AddComponent<StarterUnitCard>();
        //     card.AddComponent<EnemyUnitSlotConstraint>();
        //     card.AddComponent<NameComponent>();
        //     card.AddComponent<CardVisualComponent>();
        //     card.AddComponent<Draggable>().CanDrag = true;
        //     card.AddComponent<EnergyCost>().Cost = 1;
        //
        //
        //     string cardstr = Serializer.Serialize(card);
        //     File.WriteAllText(
        //         @"C:\Users\Arthu\Documents\Projects\DeckBuilder\Cards\SummerJam1\Prefabs\templateCard.json", cardstr);
        //     IEntity restored = Serializer.Deserialize<IEntity>(cardstr);
        //
        //     Assert.That(restored.GetComponent<Card>(), Is.Not.Null);
        // }

        // [Test]
        // public void Serialize_Objective()
        // {
        //     var obj = Context.CreateEntity();
        //     obj.AddComponent<TakeNoDamage>();
        //     obj.AddComponent<DescriptionComponent>();
        //     obj.AddComponent<VisualComponent>();
        //
        //
        //     string cardstr = Serializer.Serialize(obj);
        //     File.WriteAllText(
        //         @"C:\Users\Arthu\Documents\Projects\DeckBuilder - Copy (2)\Cards\SummerJam1\Prefabs\templateObjective.json", cardstr);
        //     IEntity restored = Serializer.Deserialize<IEntity>(cardstr);
        //
        //     Assert.That(restored.GetComponent<Objective>(), Is.Not.Null);
        // }

        // [Test]
        // public void Serialize_All_Relics()
        // {
        //     string nspace = "SummerJam1.Relics";
        //
        //     IEnumerable<Type> q = from t in Assembly.GetAssembly(typeof(VisualComponent)).GetTypes()
        //         where t.IsClass && t.Namespace == nspace && !t.IsGenericType
        //         select t;
        //
        //     foreach (Type type in q)
        //     {
        //         var obj = Context.CreateEntity();
        //         ((ChildrenCollection<Component>)obj.Components).Add((Component)Activator.CreateInstance(type));
        //         obj.AddComponent<DescriptionComponent>();
        //         obj.AddComponent<VisualComponent>();
        //         obj.AddComponent<RelicComponent>();
        //         
        //         string cardstr = Serializer.Serialize(obj);
        //         File.WriteAllText(
        //             $@"C:\Users\Arthu\Documents\Projects\DeckBuilder - Copy (2)\Cards\SummerJam1\Prefabs\Relics\{type.Name}.json", cardstr);
        //
        //     }
        // }

        // [Test]
        // public void Serialize_Unit()
        // {
        //     IEntity unit = MakeUnit();
        //     unit.AddComponent<UnitVisualComponent>().AssetName = SummerJam1UnitAsset.Noodles;
        //     var health = unit.AddComponent<Health>();
        //     unit.AddComponent<ChangeVisualOnTransform>().UnitAsset = "asset name";
        //     health.SetHealth(999);
        //     health.SetMax(999);
        //     var strength = unit.AddComponent<Strength>();
        //     unit.AddComponent<GainHealthOnTransform>().HealthToAdd = 10;
        //     unit.AddComponent<DamageIntent>();
        //     strength.Amount = 888;
        //
        //     unit.AddComponent<TransformAfterTurns>();
        //
        //     string template = Serializer.Serialize(unit);
        //
        //
        //     File.WriteAllText(
        //         @"C:\Users\Arthu\Documents\Projects\DeckBuilder\Cards\SummerJam1\Prefabs\templateUnit.json", template);
        //     IEntity restored = Serializer.Deserialize<IEntity>(template);
        //
        //     Assert.That(restored.GetComponent<Unit>(), Is.Not.Null);
        // }

        [Test]
        public void TestEventHandlerDetachmentDuringEvent()
        {
            IEntity test1 = Context.CreateEntity();
            IEntity test2 = Context.CreateEntity();
            Game.StartBattle("Units/Donut.json");
            TestEvents events1 = test1.AddComponent<TestEvents>();
            TestEvents events2 = test2.AddComponent<TestEvents>();

            bool didExecute1 = false;
            bool didExecute2 = false;
            events1.ToDo = () =>
            {
                didExecute1 = true;
                test2.Destroy();
            };
            events2.ToDo = () =>
            {
                didExecute2 = true;
                // Assert.IsTrue(events2.Entity.State == LifecycleState.Destroyed);
                // Assert.Fail("Second events executed.");
            };

            Game.EndTurn();
            Assert.IsTrue(didExecute1);
            Assert.IsFalse(didExecute2);
        }

        private event TesterEvent Tester;

        [Test]
        public void TestEventDetachmentDuringEvent_Does_Not_Work()
        {
            Tester = null;
            Tester += OnTester;
            Tester += OnTester2;

            Tester.Invoke();
            Assert.Fail();

            void OnTester()
            {
                Tester -= OnTester2;
            }

            void OnTester2()
            {
                Assert.Pass();
            }
        }

        [Test]
        public void BattleStartedByEnteringCell()
        {
            MapCreatorComponent mapCreatorCreator = Context.Root.AddComponent<MapCreatorComponent>();

            bool battleStarted = false;
            ((SummerJam1Events)Context.Events).SubscribeToBattleStarted(Action);
            CustomMap map = Context.Root.GetComponent<CustomMap>();
            Assert.NotNull(map);

            Assert.LessOrEqual(map.Height * map.Width, map.Entity.Children.Count);

            CustomCell walkableCell = map.GetAllCells().First(cell => cell.IsWalkable);
            var encounterEntity = Context.CreateEntity(walkableCell.Entity, (entity) => { entity.AddComponent<Position>(); });
            encounterEntity.AddComponent<BattleEncounter>().Prefab = "DefaultEncounter.json";

            var player = Context.CreateEntity(walkableCell.Entity, entity =>
            {
                entity.AddComponent<Position>();
                entity.AddComponent<Player>();
            });

            Assert.That(player.Parent, Is.EqualTo(walkableCell.Entity));
            Assert.That(player.GetComponent<Position>().Position1.X, Is.EqualTo(walkableCell.X));
            Assert.That(player.GetComponent<Position>().Position1.Z, Is.EqualTo(walkableCell.Y));
            Assert.IsTrue(battleStarted);

            void Action(object sender, BattleStartedEventArgs item)
            {
                battleStarted = true;
            }
        }

        [Test]
        public void CellsContainBlockedCells()
        {
            MapCreatorComponent mapCreatorCreator = Context.Root.AddComponent<MapCreatorComponent>();

            bool battleStarted = false;
            CustomMap map = Context.Root.GetComponent<CustomMap>();
            Assert.NotNull(map);

            Assert.LessOrEqual(map.Height * map.Width, map.Entity.Children.Count);

            CustomCell blockedCell = map.GetAllCells().First(cell => !cell.IsWalkable);
        }


        private UnitCard MakeUnitCard()
        {
            IEntity unitCardEntity = Context.CreateEntity();
            unitCardEntity.TrySetParent(Game.Entity);
            StarterUnitCard unitCard = unitCardEntity.AddComponent<StarterUnitCard>();
            unitCardEntity.AddComponent<NameComponent>();

            return unitCard;
        }

        private IEntity MakeUnit()
        {
            IEntity unitCardEntity = Context.CreateEntity();
            unitCardEntity.TrySetParent(Game.Entity);
            unitCardEntity.AddComponent<NameComponent>();
            unitCardEntity.AddComponent<StarterUnit>();

            return unitCardEntity;
        }
    }

    internal delegate void TesterEvent();

    public class TestUnitCard : UnitCard
    {
        protected override Unit CreateUnit()
        {
            IEntity unit = Context.CreateEntity();
            return unit.AddComponent<TestUnit>();
        }

        [OnCardPlayed]
        private void OnCardPlayed()
        {
            Logging.Log("tester.");
        }
    }

    public class TestUnit : Unit
    {
        public override bool AcceptsParent(IEntity parent)
        {
            return true;
        }
    }

    public class TestEvents : Component
    {
        public Action ToDo { get; set; }

        [OnTurnEnded]
        private void OnTurnEnded()
        {
            ToDo.Invoke();
        }
    }
}
