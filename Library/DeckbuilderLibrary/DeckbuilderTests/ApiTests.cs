﻿using System.Collections.Generic;
using System.Linq;
using Content.Cards;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Resources;
using JsonNet.ContractResolvers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DeckbuilderTests
{
    internal class ApiTests
    {
        private static IContext Context { get; set; }

        readonly JsonSerializerSettings m_JsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = new PrivateSetterContractResolver(),
            Converters = new List<JsonConverter>
            {
                new GameEntityConverter()
            }
        };

        [SetUp]
        public void Setup()
        {
            Context = new GameContext();
            PlayerActor player = (PlayerActor) Context.CreateActor<PlayerActor>(100, 0);
            Actor enemy = Context.CreateActor<BasicEnemy>(100, 0);
            Deck deck = CreateDeck(Context);

            IBattle battle = Context.CreateBattle(deck, player);
            battle.AddEnemy(enemy);

            Context.SetCurrentBattle(battle);
        }
        
        [Test]
        public void Energy_Resets_Every_Turn()
        {
            Card damage = FindCardInDeck(nameof(Attack10DamageExhaust));
            var player = Context.GetCurrentBattle().Player;
            Assert.That(player.Resources.HasResource<BaseEnergy>());
            Assert.That(player.BaseEnergy, Is.EqualTo(3));
            Assert.That(player.CurrentEnergy, Is.EqualTo(3));
            Assert.That(player.Resources.GetResourceAmount<BaseEnergy>(), Is.EqualTo(3));
            Assert.That(player.Resources.GetResourceAmount<Energy>(), Is.EqualTo(3));
            damage.PlayCard(damage.GetValidTargets().First());
            
            Assert.That(player.BaseEnergy, Is.EqualTo(3));
            Assert.That(player.CurrentEnergy, Is.EqualTo(2));
            Assert.That(player.Resources.GetResourceAmount<BaseEnergy>(), Is.EqualTo(3));
            Assert.That(player.Resources.GetResourceAmount<Energy>(), Is.EqualTo(2));
            
            Context.Events.InvokeTurnEnded(this, new TurnEndedEventArgs());
            Assert.That(player.BaseEnergy, Is.EqualTo(3));
            Assert.That(player.CurrentEnergy, Is.EqualTo(3));
            
            Assert.That(player.Resources.GetResourceAmount<BaseEnergy>(), Is.EqualTo(3));
            Assert.That(player.Resources.GetResourceAmount<Energy>(), Is.EqualTo(3));
           
        }

        [Test]
        public void DamageDoubleCard_DoublesDamage()
        {
            Card doubler = FindCardInDeck(nameof(DoubleNextCardDamage));
            Card damage = FindCardInDeck(nameof(Attack5Damage));

            var enemy = Context.GetEnemies().First();
            string text = damage.GetCardText(null);
            //enemy starts with 100 health
            Assert.That(enemy, Has.Property("Health").EqualTo(100));
            damage.PlayCard(enemy);
            //this card always deals 5 damage
            Assert.That(enemy, Has.Property("Health").EqualTo(95));

            //check that its text says it deals 5 damage
            Assert.That(text, Does.StartWith("Deal 5 "));
            //play card that doubles the next cards damage
            doubler.PlayCard(null);
            text = damage.GetCardText(null);
            //now see that the damage reads "10" instead of 5.
            Assert.That(text, Does.StartWith("Deal 10 "));

            //now verify it actually deals 10 instead of five.
            damage.PlayCard(enemy);
            Assert.That(enemy, Has.Property("Health").EqualTo(85));
            //and that the text has reset.
            text = damage.GetCardText(null);
            Assert.That(text, Does.StartWith("Deal 5 "));
        }


        [Test]
        public void EnemyKilled_Event_Works()
        {
            bool receivedEvent = false;
            Card damage = FindCardInDeck(nameof(Attack5Damage));
            var enemy = Context.GetEnemies().First();
            Context.Events.ActorDied += EventsOnActorDied;
            for (int i = 0; i < 20; i++)
            {
                damage.PlayCard(enemy);
            }

            Assert.That(receivedEvent);

            void EventsOnActorDied(object sender, ActorDiedEventArgs args)
            {
                receivedEvent = true;
            }
        }

        [Test]
        public void EnemyAttacksOnTurnEnd()
        {
            Assert.That(Context.GetCurrentBattle().Player, Has.Property("Health").EqualTo(100));
            Context.Events.InvokeTurnEnded(this, new TurnEndedEventArgs());
            Assert.That(Context.GetCurrentBattle().Player, Has.Property("Health").EqualTo(95));
        }


        [Test]
        public void BattleEnds_WhenAllEnemiesKilled()
        {
            Context.Events.BattleEnded += EventsOnBattleEnded;
            bool receivedEvent = false;
            Card damage = FindCardInDeck(nameof(Attack5Damage));
            IActor enemy = Context.GetEnemies().First();
            int test = 0;
            for (int i = 0; i < 20; i++)
            {
                damage.PlayCard(enemy);
                test = i;
            }
            
            Assert.That(test, Is.EqualTo(19));

            Assert.That(enemy.Health, Is.EqualTo(0));
            Assert.That(receivedEvent);
            void EventsOnBattleEnded(object sender, BattleEndedEventArgs args)
            {
                receivedEvent = true;
                Assert.That(args.IsVictory);
            }
        }


        private Deck CreateDeck(IContext context)
        {
            Deck deck = context.CreateEntity<Deck>();
            foreach (Card card in CreateCards(context))
            {
                deck.DrawPile.Cards.Add(card);
            }

            return deck;
        }

        private IEnumerable<Card> CreateCards(IContext context)
        {
            yield return context.CreateEntity<Attack5Damage>();
            yield return context.CreateEntity<DoubleNextCardDamage>();
            yield return context.CreateEntity<Attack10DamageExhaust>();
            yield return context.CreateEntity<DealMoreDamageEachPlay>();
        }

        [Test]
        public void TestThatSetupWorks()
        {
            //Check that setup is working
            Assert.That(Context.GetPlayerHealth(), Is.EqualTo(100));
            IReadOnlyList<IActor> enemies = Context.GetEnemies();
            Assert.That(enemies, Has.Count.EqualTo(1));
        }


        [Test]
        public void TestGetValidTargets()
        {
            Card card = FindCardInDeck("Attack5Damage");
            IReadOnlyList<IActor> targets = card.GetValidTargets();

            Assert.That(targets, Has.Count.EqualTo(Context.GetEnemies().Count));
            Assert.That(targets, Has.Count.EqualTo(1));
            IActor firstTarget = targets[0];
            IActor firstEnemy = Context.GetEnemies()[0];
            Assert.That(firstTarget.Armor, Is.EqualTo(firstEnemy.Armor));
            Assert.That(firstTarget.Health, Is.EqualTo(firstEnemy.Health));
            Assert.That(firstTarget.Id, Is.EqualTo(firstEnemy.Id));
            Assert.That(firstTarget, Is.EqualTo(firstEnemy));
        }


        [Test]
        public void TestAttackDealsDamage()
        {
            Card card = FindCardInDeck("Attack5Damage");
            var targets = card.GetValidTargets();
            Assert.That(targets[0].Health, Is.EqualTo(100));
            card.PlayCard(targets[0]);
            Assert.That(targets[0].Health, Is.LessThan(100));
        }


        [Test]
        public void TestAttackCausesEvent()
        {
            bool gotEvent = false;
            Card card = FindCardInDeck("Attack5Damage");
            IReadOnlyList<IActor> targets = card.GetValidTargets();
            Context.Events.DamageDealt += GameEventHandlerOnDamageDealt;
            Assert.That(targets[0].Health, Is.EqualTo(100));
            card.PlayCard(targets[0]);
            Assert.That(targets[0].Health, Is.LessThan(100));
            Assert.That(gotEvent);


            void GameEventHandlerOnDamageDealt(object sender, DamageDealtArgs args)
            {
                gotEvent = true;
                Assert.That(args.HealthDamage, Is.EqualTo(5));
                Assert.That(args.TotalDamage, Is.EqualTo(5));
                Assert.That(args.ActorId, Is.EqualTo(targets[0].Id));
            }
        }


        [Test]
        public void TestCardIsDiscarded()
        {
            bool receivedMoveEvent = false;
            Card cardToPlay = FindCardInDeck("Attack5Damage");
            IActor target = cardToPlay.GetValidTargets()[0];
            Context.Events.CardMoved += DeckOnOnCardMoved;
            cardToPlay.PlayCard(target);
            Assert.That(Context.GetCurrentBattle().Deck.DrawPile.Cards, !Contains.Item(cardToPlay));
            Assert.That(Context.GetCurrentBattle().Deck.DiscardPile.Cards, Contains.Item(cardToPlay));
            if (!receivedMoveEvent)
            {
                Assert.Fail("Never received event for card move!");
            }

            void DeckOnOnCardMoved(object sender, CardMovedEventArgs args)
            {
                receivedMoveEvent = true;
                Assert.That(args.MovedCard, Is.EqualTo(cardToPlay.Id));
                Assert.That(args.NewPileType, Is.EqualTo(PileType.DiscardPile));
                Assert.That(args.PreviousPileType, Is.EqualTo(PileType.DrawPile));
            }
        }


        [Test]
        public void TestCardIsExhausted()
        {
            bool receivedMoveEvent = false;
            Card cardToPlay = FindCardInDeck("Attack10DamageExhaust");
            IActor target = cardToPlay.GetValidTargets()[0];
            Context.Events.CardMoved += DeckOnCardMoved;
            cardToPlay.PlayCard(target);
            Assert.That(Context.GetCurrentBattle().Deck.DrawPile.Cards, !Contains.Item(cardToPlay));
            Assert.That(Context.GetCurrentBattle().Deck.ExhaustPile.Cards, Contains.Item(cardToPlay));
            if (!receivedMoveEvent)
            {
                Assert.Fail("Never received event for card move!");
            }

            void DeckOnCardMoved(object sender, CardMovedEventArgs args)
            {
                receivedMoveEvent = true;
                Assert.That(args.MovedCard, Is.EqualTo(cardToPlay.Id));
                Assert.That(args.NewPileType, Is.EqualTo(PileType.ExhaustPile));
                Assert.That(args.PreviousPileType, Is.EqualTo(PileType.DrawPile));
            }
        }

        private Card FindCardInDeck(string name)
        {
            return Context.GetCurrentBattle().Deck.AllCards().First(card => card.Name == name);
        }

        private Card FindCardInDeck(int cardId)
        {
            return Context.GetCurrentBattle().Deck.AllCards().First(card => card.Id == cardId);
        }


        [Test]
        public void TestMultipleGameSimulationsDontOverlap()
        {
            var copiedContext = GetCopiedContext();
            Assert.That(copiedContext.GetCurrentBattle().Deck.AllCards().ToList(), Has.Count.GreaterThan(0));
            Assert.That(copiedContext.GetEnemies().ToList(), Has.Count.GreaterThan(0));
            Assert.That(copiedContext.GetCurrentBattle().Deck.AllCards().FirstOrDefault(c => c.Id > 0), Is.Not.Null);
            Assert.That(copiedContext.GetCurrentBattle().Enemies.First().Id,
                Is.EqualTo(Context.GetCurrentBattle().Enemies.First().Id));

            Card card = FindCardInDeck("DealMoreDamageEachPlay");
            IReadOnlyList<IActor> targets = card.GetValidTargets();
            Assert.That(targets[0].Health, Is.EqualTo(100));
            card.PlayCard(targets[0]);
            Assert.That(targets[0].Health, Is.EqualTo(99));

            Assert.That(copiedContext.GetEnemies().First().Health, Is.EqualTo(100));
            Card copiedCard = copiedContext.GetCurrentBattle().Deck.AllCards().First(c => c.Id == card.Id);
            IReadOnlyList<IActor> copiedTargets = copiedCard.GetValidTargets();
            copiedCard.PlayCard(copiedTargets[0]);
            Assert.That(copiedContext.GetEnemies().First().Health, Is.EqualTo(99)); //should have only dealt 1 damage
        }

        [Test]
        public void TestCopiedContextGetsSerializedData()
        {
            Card card = FindCardInDeck("DealMoreDamageEachPlay");
            IReadOnlyList<IActor> targets = card.GetValidTargets();
            Assert.That(targets[0].Health, Is.EqualTo(100));
            card.PlayCard(targets[0]);
            Assert.That(targets[0].Health, Is.EqualTo(99));

            var copiedContext = GetCopiedContext();
            GameContext.CurrentContext = copiedContext;
            Assert.That(copiedContext.GetEnemies().First().Health, Is.EqualTo(99));

            Card copiedCard = copiedContext.GetCurrentBattle().Deck.AllCards().First(c => c.Id == card.Id);
            Assert.That(copiedCard.Context, Is.Not.Null);
            IReadOnlyList<IActor> copiedTargets = copiedCard.GetValidTargets();
            copiedCard.PlayCard(copiedTargets[0]);
            Assert.That(copiedContext.GetEnemies().First().Health,
                Is.EqualTo(97)); //context copied after the first play so it should deal 2 damage.
        }


        private GameContext GetCopiedContext()
        {
            string contextStr = JsonConvert.SerializeObject(Context, m_JsonSerializerSettings);
            GameContext copy = JsonConvert.DeserializeObject<GameContext>(contextStr, m_JsonSerializerSettings);
            return copy;
        }
    }
}