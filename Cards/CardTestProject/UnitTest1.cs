using Api;
using NUnit.Framework;

namespace Tests
{
    public class Tests
    {
        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            //Create a game
            Entity game = new Entity();
            game.Components.Add(new Events());
            game.Initialize();
            
            //Add entity with test components
            Entity entity = new Entity();
            var testComponent = new CardPlayedComponent();
            entity.Components.Add(testComponent);
            
            var testComponent2 = new CardDiscardedComponent();
            entity.Components.Add(testComponent2);
            entity.SetParent(game);
            
            //Verify initial value.
            Assert.IsFalse(testComponent.CardPlayed);
            Assert.IsFalse(testComponent2.CardDiscarded);
            
            //Simulate a card being played
            game.GetComponent<Events>().OnCardPlayed(new Events.CardPlayedEventArgs(0, 0));
            
            //Verify event was fired from attribute.
            Assert.IsTrue(testComponent.CardPlayed);
            
            //Simulate a card being discarded
            game.GetComponent<Events>().OnCardDiscarded(new Events.CardDiscardedEventArgs(0, 0));
            
            
            //Verify event was fired from attribute.
            Assert.IsTrue(testComponent2.CardDiscarded);

        }
    }
}