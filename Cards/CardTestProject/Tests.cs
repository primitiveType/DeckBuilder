using System.Linq;
using System.Xml.Serialization;
using Api;
using Api.Components;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        private Context Context { get; set; }

        [SetUp]
        public void Setup()
        {
            Context = new Context();
        }

        [Test]
        public void Test1()
        {
            //Create a game
            IEntity game = Context.CreateEntity();
            game.AddComponent<Events>();

            //Add entity with test components
            IEntity entity = Context.CreateEntity();
            entity.SetParent(game);

            var testComponent = entity.AddComponent<CardPlayedComponent>();

            var testComponent2 = entity.AddComponent<CardDiscardedComponent>();

            //Verify initial value.
            Assert.IsFalse(testComponent.CardPlayed);
            Assert.IsFalse(testComponent2.CardDiscarded);

            //Simulate a card being played
            game.GetComponent<Events>().OnCardPlayed(new CardPlayedEventArgs(0, 0));

            //Verify event was fired from attribute.
            Assert.IsTrue(testComponent.CardPlayed);

            //Simulate a card being discarded
            game.GetComponent<Events>().OnCardDiscarded(new CardDiscardedEventArgs(0, 0));


            //Verify event was fired from attribute.
            Assert.IsTrue(testComponent2.CardDiscarded);
        }

        [Test]
        public void TestDealDamage()
        {
            //Create a game
            IEntity game = Context.CreateEntity();
            game.AddComponent<Events>();

            //Add entity with test components
            IEntity entity = Context.CreateEntity();
            Health health = entity.AddComponent<Health>();
            Assert.That(health.Amount, Is.EqualTo(10));
            entity.SetParent(game);


            RequestDealDamageEventArgs
                args = new RequestDealDamageEventArgs(3, entity, entity); //stop hitting yourself!
            game.GetComponent<Events>().OnRequestDealDamage(args);


            Assert.That(health.Amount, Is.EqualTo(7));

            RequestDealDamageEventArgs
                args2 = new RequestDealDamageEventArgs(30, entity, entity); //stop hitting yourself!
            game.GetComponent<Events>().OnRequestDealDamage(args2);
            Assert.That(health.Amount, Is.EqualTo(0));
        }

        [Test]
        public void TestPredictDamage()
        {
            //Create a game
            IEntity game = Context.CreateEntity();
            game.AddComponent<Events>();

            //Add entity with test components
            IEntity entity = Context.CreateEntity();
            Health health = entity.AddComponent<Health>();
            Assert.That(health.Amount, Is.EqualTo(10));
            entity.SetParent(game);


            var gameString = Serializer.Serialize(game);
            var gameCopy = Serializer.Deserialize<Entity>(gameString);

            var healthCopy = gameCopy.Children.First().GetComponent<Health>();

            Assert.NotNull(healthCopy);
            Assert.AreEqual(healthCopy.Amount, health.Amount);

            RequestDealDamageEventArgs
                args2 = new RequestDealDamageEventArgs(30, entity, entity); //stop hitting yourself!
            gameCopy.GetComponent<Events>().OnRequestDealDamage(args2);


            Assert.NotNull(healthCopy);
            Assert.AreNotEqual(healthCopy.Amount, health.Amount);
            Assert.NotNull(healthCopy.Parent);
            Assert.AreEqual(healthCopy.Parent.Id, health.Parent.Id);
        }

        [Test]
        public void TestPreventDamage()
        {
            //Create a game
            IEntity game = Context.CreateEntity();
            game.AddComponent<Events>();

            //Add entity with test components
            IEntity entity = Context.CreateEntity(game);
            Health health = entity.AddComponent<Health>();
            //Prevent the next source of damage.
            entity.AddComponent<PreventAllDamageOnceComponent>();
            Assert.That(health.Amount, Is.EqualTo(10));


            RequestDealDamageEventArgs
                args2 = new RequestDealDamageEventArgs(30, entity, entity); //stop hitting yourself!
            game.GetComponent<Events>().OnRequestDealDamage(args2);
            //damage should have been prevented.
            Assert.That(health.Amount, Is.EqualTo(10));

            RequestDealDamageEventArgs
                args3 = new RequestDealDamageEventArgs(30, entity, entity); //stop hitting yourself!
            game.GetComponent<Events>().OnRequestDealDamage(args3);
            //damage should not have been prevented.
            Assert.That(health.Amount, Is.EqualTo(0));
        }
    }
}