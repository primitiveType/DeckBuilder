using System.Linq;
using Api;
using CardsAndPiles;
using CardsAndPiles.Components;
using NUnit.Framework;

namespace CardTestProject
{
    public class Tests
    {
        private Context Context { get; set; }
        private CardEvents Events => (CardEvents)Context.Events;

        [SetUp]
        public void Setup()
        {
            Context = new Context(new CardEvents());
        }

        [Test]
        public void Test1()
        {
            //Create a game
            IEntity game = Context.CreateEntity();

            //Add entity with test components
            IEntity entity = Context.CreateEntity();
            entity.TrySetParent(game);

            var testComponent = entity.AddComponent<CardPlayedComponent>();

            var testComponent2 = entity.AddComponent<CardDiscardedComponent>();

            //Verify initial value.
            Assert.IsFalse(testComponent.CardPlayed);
            Assert.IsFalse(testComponent2.CardDiscarded);

            //Simulate a card being played
            Events.OnCardPlayed(new CardPlayedEventArgs(null, game));

            //Verify event was fired from attribute.
            Assert.IsTrue(testComponent.CardPlayed);

            //Simulate a card being discarded
            Events.OnCardDiscarded(new CardDiscardedEventArgs(null));


            //Verify event was fired from attribute.
            Assert.IsTrue(testComponent2.CardDiscarded);
        }

        [Test]
        public void TestDealDamage()
        {
            //Create a game
            IEntity game = Context.CreateEntity();

            //Add entity with test components
            IEntity entity = Context.CreateEntity(game);
            Health health = entity.AddComponent<Health>();
            Assert.That(health.Amount, Is.EqualTo(10));
            entity.TrySetParent(game);


            RequestDealDamageEventArgs
                args = new RequestDealDamageEventArgs(3, entity, entity); //stop hitting yourself!
            Events.OnRequestDealDamage(args);


            Assert.That(health.Amount, Is.EqualTo(7));

            RequestDealDamageEventArgs
                args2 = new RequestDealDamageEventArgs(30, entity, entity); //stop hitting yourself!
            Events.OnRequestDealDamage(args2);
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
            Assert.That(health.Amount, Is.EqualTo(10));
            entity.TrySetParent(game);


            var gameString = Serializer.Serialize(Context);
            var gameCopy = Serializer.Deserialize<Context>(gameString);

            var healthCopy = gameCopy.Root.Children.First().GetComponent<Health>();

            Assert.NotNull(healthCopy);
            Assert.AreEqual(healthCopy.Amount, health.Amount);

            RequestDealDamageEventArgs
                args2 = new RequestDealDamageEventArgs(30, entity, entity); //stop hitting yourself!
            Events.OnRequestDealDamage(args2);


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
            //Prevent the next source of damage.
            entity.AddComponent<PreventAllDamageOnceComponent>();
            Assert.That(health.Amount, Is.EqualTo(10));


            RequestDealDamageEventArgs
                args2 = new RequestDealDamageEventArgs(30, entity, entity); //stop hitting yourself!
            Events.OnRequestDealDamage(args2);
            //damage should have been prevented.
            Assert.That(health.Amount, Is.EqualTo(10));

            RequestDealDamageEventArgs
                args3 = new RequestDealDamageEventArgs(30, entity, entity); //stop hitting yourself!
            Events.OnRequestDealDamage(args3);
            //damage should not have been prevented.
            Assert.That(health.Amount, Is.EqualTo(0));
        }
    }
}