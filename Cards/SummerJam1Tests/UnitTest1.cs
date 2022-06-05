using System;
using Api;
using CardsAndPiles;
using NUnit.Framework;
using SummerJam1;

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
            var gameEntity = Context.CreateEntity();
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

        private UnitCard MakeUnitCard()
        {
            var unitCardEntity = Context.CreateEntity();
            unitCardEntity.TrySetParent(Game.Entity);
            var unitCard = unitCardEntity.AddComponent<TestUnitCard>();

            return unitCard;
        }
    }

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
}