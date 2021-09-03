using System.Linq;
using System.Runtime.Remoting.Contexts;
using Content.Cards;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Battles;
using NUnit.Framework;

namespace DeckbuilderTests
{
    internal class AdjacencyTests : BaseTest
    {
        [SetUp]
        public void Setup()
        {
            Context = new GameContext();


            Player = Context.CreateActor<PlayerActor>(100, 0);
            CreateDeck(Context);
        }

        [Test]
        public void TestGetAdjacentEnemies()
        {
            Context.StartBattle(Player, Context.CreateEntity<TwoEnemyBattleData>());
            Assert.That(Context.GetEnemies().Count, Is.EqualTo(2));
            Assert.That(Context.GetCurrentBattle().GetAdjacentActors(Player).Count, Is.EqualTo(1));
        }

        [Test]
        public void TestMovement()
        {
            Context.StartBattle(Player, Context.CreateEntity<BasicBattleData>());
            Assert.That(Context.GetEnemies().Count, Is.EqualTo(1));
            Assert.That(Context.GetCurrentBattle().GetAdjacentActors(Player).Count, Is.EqualTo(0));
            var moveCard = FindCardInDeck(nameof(MoveToEmptyAdjacentNode));
            moveCard.PlayCard(Context.GetCurrentBattle().GetAdjacentEmptyNodes(Player).First());
            Assert.That(Context.GetCurrentBattle().GetAdjacentActors(Player).Count, Is.EqualTo(1));
        }

        [Test]
        public void TestMovementEvent()
        {
            bool gotEvent = false;
            Context.Events.ActorsSwapped += OnActorsSwapped;
            Context.StartBattle(Player, Context.CreateEntity<BasicBattleData>());
            Assert.That(Context.GetEnemies().Count, Is.EqualTo(1));
            Assert.That(Context.GetCurrentBattle().GetAdjacentActors(Player).Count, Is.EqualTo(0));
            var moveCard = FindCardInDeck(nameof(MoveToEmptyAdjacentNode));
            moveCard.PlayCard(Context.GetCurrentBattle().GetAdjacentEmptyNodes(Player).First());
            Assert.That(Context.GetCurrentBattle().GetAdjacentActors(Player).Count, Is.EqualTo(1));

            Assert.That(gotEvent);
            void OnActorsSwapped(object sender, ActorsSwappedEventArgs args)
            {
                gotEvent = true;
                Assert.That(args.Actor1 == null || args.Actor2 == null);
            }
        }
    }
}