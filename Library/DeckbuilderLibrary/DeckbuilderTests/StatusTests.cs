using System.Linq;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Actors.Test;
using DeckbuilderLibrary.Data.GameEntities.Battles.TestBattles;
using DeckbuilderLibrary.Data.GameEntities.Resources.Status;
using NUnit.Framework;

namespace DeckbuilderTests
{
    internal class StatusTests : BaseTest
    {
        [SetUp]
        public void Setup()
        {
            Context = new GameContext();


            Player = Context.CreateActor<PlayerActor>(100, 0);
            CreateDeck(Context);


            Context.StartBattle(Player, Context.CreateEntity<TestBattleDataWithDummyEnemy>());
        }

        public DummyEnemy Enemy { get; set; }

        [Test]
        public void PoisonDealsDamageAndDecrements()
        {
            var enemy = Context.GetCurrentBattle().Enemies.First();
            enemy.Resources.AddResource<PoisonStatusEffect>(5);
            Assert.That(enemy.Resources.GetResourceAmount<PoisonStatusEffect>(), Is.EqualTo(5));

            Assert.That(enemy, Has.Property("Health").EqualTo(100));
            Context.EndTurn();
            Assert.That(enemy, Has.Property("Health").EqualTo(100 - 5));
            Context.EndTurn();
            Assert.That(enemy, Has.Property("Health").EqualTo(100 - 5 - 4));
            Context.EndTurn();
            Assert.That(enemy, Has.Property("Health").EqualTo(100 - 5 - 4 - 3));
            Context.EndTurn();
            Assert.That(enemy, Has.Property("Health").EqualTo(100 - 5 - 4 - 3 - 2));
            Context.EndTurn();
            Assert.That(enemy, Has.Property("Health").EqualTo(100 - 5 - 4 - 3 - 2 - 1));

            //poison should be gone at this point
            Context.EndTurn();
            Assert.That(enemy, Has.Property("Health").EqualTo(100 - 5 - 4 - 3 - 2 - 1));

            Assert.That(enemy.Resources.GetResourceAmount<PoisonStatusEffect>(), Is.EqualTo(0));
        }
        
        [Test]
        public void VulnerableIncreasesDamageDealt()
        {
            Card card = FindCardInDeck("TestAttack5Damage");
            ActorNode target = (ActorNode)card.GetValidTargets().First();
            Assert.That(target.GetActor().Health, Is.EqualTo(100));
            card.PlayCard(target);
            Assert.That(target.GetActor().Health, Is.EqualTo(95));
            var player = Context.GetCurrentBattle().Player;
            target.GetActor().Resources.AddResource<VulnerableStatusEffect>(3);
            card.PlayCard(target);
            Assert.That(target.GetActor().Health, Is.EqualTo(88)); //should deal 7 if target is vulnerable.
        }
        
        
        [Test]
        public void WeakReducesDamageDealt()
        {
            Card card = FindCardInDeck("TestAttack5Damage");
            ActorNode target = (ActorNode)card.GetValidTargets().First();
            Assert.That(target.GetActor().Health, Is.EqualTo(100));
            card.PlayCard(target);
            Assert.That(target.GetActor().Health, Is.EqualTo(95));
            var player = Context.GetCurrentBattle().Player;
            player.Resources.AddResource<WeakStatusEffect>(3);
            card.PlayCard(target);
            Assert.That(target.GetActor().Health, Is.EqualTo(92)); //should only deal 3 if player is weak.
        }
    }
}