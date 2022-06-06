using System;
using Api;
using CardsAndPiles;
using NUnit.Framework;
using SummerJam1;
using SummerJam1.Units;

namespace SummerJam1Tests
{
    public class Tests
    {
        private Context Context { get; set; }
        private SummerJam1Game Game { get; set; }

        [SetUp]
        public void Setup()
        {
            Context = new Context(new SummerJam1Events());
            var gameEntity = Context.Root;
            Game = gameEntity.AddComponent<SummerJam1Game>();
        }

        [Test]
        public void TestUnitCreatedInSlot()
        {
            var unitCard = MakeUnitCard();
            var unitSlot = Game.Entity.GetComponentInChildren<FriendlyUnitSlot>();

            Assert.NotNull(unitSlot);
            Assert.IsTrue(unitCard.TryPlayCard(unitSlot.Entity));

            Assert.NotNull(unitSlot.Entity.GetComponentInChildren<Unit>());

            var unitCard2 = MakeUnitCard();
            Assert.IsFalse(unitCard2.TryPlayCard(unitSlot.Entity));
        }


        [Test]
        public void TestEventHandlerDetachmentDuringEvent()
        {
            var test1 = Context.CreateEntity();
            var test2 = Context.CreateEntity();

            var events1 = test1.AddComponent<TestEvents>();
            var events2 = test2.AddComponent<TestEvents>();

            bool didExecute1 = false;
            bool didExecute2 = false;
            events1.ToDo = () =>
            {
                didExecute1 = true;
                test2.Destroy();
            };
            events2.ToDo = ()=>
            {
                didExecute2 = true;
                // Assert.IsTrue(events2.Entity.State == LifecycleState.Destroyed);
                // Assert.Fail("Second events executed.");
            };
            
            Game.EndTurn();
            Assert.IsTrue(didExecute1);
            Assert.IsFalse(didExecute2);

        }

        event TesterEvent Tester;

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

        


        private UnitCard MakeUnitCard()
        {
            var unitCardEntity = Context.CreateEntity();
            unitCardEntity.TrySetParent(Game.Entity);
            var unitCard = unitCardEntity.AddComponent<TestUnitCard>();

            return unitCard;
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
            Console.WriteLine("tester.");
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