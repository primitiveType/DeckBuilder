using Api;
using CardsAndPiles;
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

            CardPlayedComponent testComponent = entity.AddComponent<CardPlayedComponent>();

            CardDiscardedComponent testComponent2 = entity.AddComponent<CardDiscardedComponent>();

            //Verify initial value.
            Assert.IsFalse(testComponent.CardPlayed);
            Assert.IsFalse(testComponent2.CardDiscarded);

            //Simulate a card being played
            Events.OnCardPlayed(new CardPlayedEventArgs(null, game, false));

            //Verify event was fired from attribute.
            Assert.IsTrue(testComponent.CardPlayed);

            //Simulate a card being discarded
            Events.OnCardDiscarded(new CardDiscardedEventArgs(null));


            //Verify event was fired from attribute.
            Assert.IsTrue(testComponent2.CardDiscarded);
        }
    }
}