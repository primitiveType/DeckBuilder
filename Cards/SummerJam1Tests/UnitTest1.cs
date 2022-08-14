using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using CardTestProject;
using NUnit.Framework;
using RogueMaps;
using SummerJam1;
using SummerJam1.Statuses;
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
        private CardEvents Events => (CardEvents)Context.Events;
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
        public void StartBattle()
        {
            Game.StartBattle();
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
        //     card.AddComponent<EnergyCost>().Amount = 1;
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
            Game.StartBattle();
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
        public void ChildrenChangesAreWrapped()
        {
            bool isWrapped = false;

            var entity = Context.CreateEntity();
            var child = Context.CreateEntity();
            
            entity.Children.CollectionChanged += ChildrenOnCollectionChanged1;
            entity.Children.CollectionChanged += ChildrenOnCollectionChanged2;


            child.TrySetParent(entity);
            
            Assert.IsTrue(isWrapped);
            void ChildrenOnCollectionChanged1(object sender, NotifyCollectionChangedEventArgs e)
            {
                throw new Exception();
            }
            void ChildrenOnCollectionChanged2(object sender, NotifyCollectionChangedEventArgs e)
            {
                isWrapped = true;
            }
            
        }
        
        [Test]
        public void CreateHeadCheese()
        {
            var cheese = Context.CreateEntity(Context.Root, "Units/Standard/2/headCheese.json");
            Assert.That(cheese.GetComponent<GainMultiAttackBelowThreshold>(), Is.Not.Null);
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

        [Test]
        public void TestDealDamage()
        {
            //Create a game
            IEntity game = Context.CreateEntity();

            //Add entity with test components
            IEntity entity = Context.CreateEntity(game);
            Health health = entity.AddComponent<Health>();
            health.Amount = 10;
            Assert.That(health.Amount, Is.EqualTo(10));
            entity.TrySetParent(game);


            health.TryDealDamage(3, entity);


            Assert.That(health.Amount, Is.EqualTo(7));

            health.TryDealDamage(30, entity);

            Assert.That(health.Amount, Is.EqualTo(0));
        }

        [Test]
        public void TestPredictDamage()
        {
            //Create a game
            IEntity game = Context.Root;

            //Add entity with test components
            IEntity entity = Context.CreateEntity(game);
            Health health = entity.AddComponent<Health>();
            health.Amount = 10;
            Assert.That(health.Amount, Is.EqualTo(10));
            entity.TrySetParent(game);


            var gameString = Serializer.Serialize(Context);
            var gameCopy = Serializer.Deserialize<Context>(gameString);

            var healthCopy = gameCopy.Root.Children.First().GetComponent<Health>();

            Assert.NotNull(healthCopy);
            Assert.AreEqual(healthCopy.Amount, health.Amount);

            RequestDamageMultipliersEventArgs
                args2 = new RequestDamageMultipliersEventArgs(30, entity, entity); //stop hitting yourself!
            Events.OnRequestDamageMultipliers(args2);


            Assert.NotNull(healthCopy);
            Assert.AreNotEqual(healthCopy.Amount, health.Amount);
            Assert.NotNull(healthCopy.Entity);
            Assert.AreEqual(healthCopy.Entity.Id, health.Entity.Id);
        }

        [Test]
        public void TestPreventDamage()
        {
            //Create a game
            IEntity game = Context.CreateEntity();

            //Add entity with test components
            IEntity entity = Context.CreateEntity(game);
            Health health = entity.AddComponent<Health>();
            health.Amount = 10;
            //Prevent the next source of damage.
            entity.AddComponent<PreventAllDamageOnceComponent>();
            Assert.That(health.Amount, Is.EqualTo(10));


            RequestDamageMultipliersEventArgs
                args2 = new RequestDamageMultipliersEventArgs(30, entity, entity); //stop hitting yourself!
            Events.OnRequestDamageMultipliers(args2);
            //damage should have been prevented.
            Assert.That(health.Amount, Is.EqualTo(10));

            RequestDamageMultipliersEventArgs
                args3 = new RequestDamageMultipliersEventArgs(30, entity, entity); //stop hitting yourself!
            Events.OnRequestDamageMultipliers(args3);
            //damage should not have been prevented.
            Assert.That(health.Amount, Is.EqualTo(0));
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

    public class TestUnit : Unit
    {
        protected override bool PlayCard(IEntity target)
        {
            return true;
        }

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
