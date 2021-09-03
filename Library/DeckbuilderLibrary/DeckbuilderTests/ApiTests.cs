using System;
using System.Collections.Generic;
using System.Linq;
using Content.Cards;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using DeckbuilderLibrary.Data.GameEntities.Battles;
using DeckbuilderLibrary.Data.GameEntities.Resources;
using DeckbuilderLibrary.Data.GameEntities.Resources.Status;
using JsonNet.ContractResolvers;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DeckbuilderTests
{
    internal class ApiTests : BaseTest
    {
        /// cases to cover:
        /// card used in combat permanently changes stat out of combat
        /// card used in combat temporarily changes stat until end of combat
        /// some effects on cards temporary until end of turn
        /// able to view cards in hand and see their temporary effects, but when viewing deck, only permanent effects appear.
        ///
        /// approach: at the start of battle, the deck is copied.
        /// cards that need to increment a permanent value store it on the context as a "rule", along with the id of the card, so that it works in a sim context.

        [SetUp]
        public void Setup()
        {
            Context = new GameContext();


            Player = (PlayerActor)Context.CreateActor<PlayerActor>(100, 0);
            CreateDeck(Context);


            IBattle battle = Context.StartBattle(Player, Context.CreateEntity<BasicBattleData>());
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

            Context.EndTurn();
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
        public void PlayerStartsWithFiveCards()
        {
            Assert.That(Context.GetCurrentBattle().Deck.HandPile.Cards.Count, Is.EqualTo(5));
        }

        [Test]
        public void DiscardShuffledIntoDraw()
        {
            Context.Events.CardMoved += OnCardMoved;
            bool receivedEvent = false;
            var drawCards = Context.GetCurrentBattle().Deck.DrawPile.Cards;
            var count = drawCards.Count;
            var target = Context.GetCurrentBattle().Enemies.First();
            Assert.That(count, Is.GreaterThan(1));
            while (drawCards.Count > 1)
            {
                drawCards[0].PlayCard(target);
                count--;
                Assert.That(drawCards.Count, Is.EqualTo(count));
            }

            Assert.That(drawCards.Count, Is.EqualTo(1));
            Assert.That(receivedEvent, Is.False);
            drawCards[0].PlayCard(target);
            Assert.That(drawCards.Count,
                Is.GreaterThan(1)); //some cards exhaust, but we should basically never have zero cards in draw.
            Assert.That(receivedEvent, Is.True);

            void OnCardMoved(object sender, CardMovedEventArgs args)
            {
                if (args.NewPileType == PileType.DrawPile)
                {
                    receivedEvent = true;
                }
            }
        }

        [Test]
        public void DrawCardsEachTurn()
        {
            Context.Events.CardMoved += OnCardMoved;
            bool receivedEvent = false;
            IPile handPile = Context.GetCurrentBattle().Deck.HandPile;

            Assert.That(handPile.Cards.Count, Is.EqualTo(5));
            Context.TrySendToPile(handPile.Cards.First().Id, PileType.DiscardPile);
            Assert.That(handPile.Cards.Count, Is.EqualTo(4));
            Context.EndTurn();

            Assert.That(handPile.Cards.Count, Is.EqualTo(5));

            Assert.That(receivedEvent);

            void OnCardMoved(object sender, CardMovedEventArgs args)
            {
                if (args.NewPileType == PileType.HandPile)
                {
                    receivedEvent = true;
                }
            }
        }


        [Test]
        public void EnemyAttacksOnTurnEnd()
        {
            Assert.That(Context.GetCurrentBattle().Player, Has.Property("Health").EqualTo(100));
            Context.EndTurn();
            Assert.That(Context.GetCurrentBattle().Player, Has.Property("Health").EqualTo(95));
        }

        [Test]
        public void WeakReducesDamageDealt()
        {
            Card card = FindCardInDeck("Attack5Damage");
            IActor target = (IActor)card.GetValidTargets().First();
            Assert.That(target.Health, Is.EqualTo(100));
            card.PlayCard(target);
            Assert.That(target.Health, Is.EqualTo(95));
            var player = Context.GetCurrentBattle().Player;
            player.Resources.AddResource<WeakStatusEffect>(3);
            card.PlayCard(target);
            Assert.That(target.Health, Is.EqualTo(92)); //should only deal 3 if player is weak.
        }

        [Test]
        public void VulnerableIncreasesDamageDealt()
        {
            Card card = FindCardInDeck("Attack5Damage");
            IActor target = (IActor)card.GetValidTargets().First();
            Assert.That(target.Health, Is.EqualTo(100));
            card.PlayCard(target);
            Assert.That(target.Health, Is.EqualTo(95));
            var player = Context.GetCurrentBattle().Player;
            target.Resources.AddResource<VulnerableStatusEffect>(3);
            card.PlayCard(target);
            Assert.That(target.Health, Is.EqualTo(88)); //should deal 7 if target is vulnerable.
        }
        

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

        [Test]
        public void MultipleBattles()
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

            Context.StartBattle(Player, Context.CreateEntity<BasicBattleData>());
            enemy = Context.GetEnemies().First();
            for (int i = 0; i < 20; i++)
            {
                damage.PlayCard(enemy);
                if (i % 5 == 0)
                {
                    Context.EndTurn();
                }

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
            IReadOnlyList<IGameEntity> targets = card.GetValidTargets();

            Assert.That(targets, Has.Count.EqualTo(Context.GetEnemies().Count));
            Assert.That(targets, Has.Count.EqualTo(1));
            IActor firstTarget = (IActor)targets[0];
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
            IActor target = (IActor)card.GetValidTargets().First();
            Assert.That(target.Health, Is.EqualTo(100));
            card.PlayCard(target);
            Assert.That(target.Health, Is.LessThan(100));
        }

        [Test]
        public void ClonedCardHasNewId()
        {
            Card card = FindCardInDeck("Attack5Damage");
            var copy = Context.CopyCard(card);
            Assert.That(copy.Id, Is.Not.EqualTo(card.Id));
        }


        [Test]
        public void TestAttackCausesEvent()
        {
            bool gotEvent = false;
            Card card = FindCardInDeck("Attack5Damage");
            IActor target = (IActor)card.GetValidTargets().First();
            Context.Events.DamageDealt += GameEventHandlerOnDamageDealt;
            Assert.That(target.Health, Is.EqualTo(100));
            card.PlayCard(target);
            Assert.That(target.Health, Is.LessThan(100));
            Assert.That(gotEvent);


            void GameEventHandlerOnDamageDealt(object sender, DamageDealtArgs args)
            {
                gotEvent = true;
                Assert.That(args.HealthDamage, Is.EqualTo(5));
                Assert.That(args.TotalDamage, Is.EqualTo(5));
                Assert.That(args.ActorId, Is.EqualTo(target.Id));
            }
        }


        [Test]
        public void TestCardIsDiscarded()
        {
            bool receivedMoveEvent = false;
            Card cardToPlay = FindCardInDeck("Attack5Damage");

            Assert.That(Context.GetCurrentBattle().Deck.HandPile.Cards, Contains.Item(cardToPlay));
            Assert.That(Context.GetCurrentBattle().Deck.DiscardPile.Cards, !Contains.Item(cardToPlay));

            IActor target = (IActor)cardToPlay.GetValidTargets()[0];
            Context.Events.CardMoved += OnCardMoved;
            cardToPlay.PlayCard(target);
            Assert.That(Context.GetCurrentBattle().Deck.HandPile.Cards, !Contains.Item(cardToPlay));
            Assert.That(Context.GetCurrentBattle().Deck.DiscardPile.Cards, Contains.Item(cardToPlay));
            if (!receivedMoveEvent)
            {
                Assert.Fail("Never received event for card move!");
            }

            void OnCardMoved(object sender, CardMovedEventArgs args)
            {
                receivedMoveEvent = true;
                Assert.That(args.MovedCard, Is.EqualTo(cardToPlay.Id));
                Assert.That(args.NewPileType, Is.EqualTo(PileType.DiscardPile));
                Assert.That(args.PreviousPileType, Is.EqualTo(PileType.HandPile));
            }
        }


        [Test]
        public void TestCardIsExhausted()
        {
            bool receivedMoveEvent = false;
            Card cardToPlay = FindCardInDeck("Attack10DamageExhaust");
            IActor target = (IActor)cardToPlay.GetValidTargets()[0];
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
                Assert.That(args.PreviousPileType, Is.EqualTo(PileType.HandPile));
            }
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
            Assert.That(copiedContext.GetCurrentBattle().Deck.AllCards().FirstOrDefault(c => c.Id > 0),
                Is.Not.Null);
            Assert.That(copiedContext.GetCurrentBattle().Enemies.First().Id,
                Is.EqualTo(Context.GetCurrentBattle().Enemies.First().Id));

            Card card = FindCardInDeck("DealMoreDamageEachPlay");
            IActor target = (IActor)card.GetValidTargets().First();
            Assert.That(target.Health, Is.EqualTo(100));
            card.PlayCard(target);
            Assert.That(target.Health, Is.EqualTo(99));

            Assert.That(copiedContext.GetEnemies().First().Health, Is.EqualTo(100));
            Card copiedCard = copiedContext.GetCurrentBattle().Deck.AllCards().First(c => c.Id == card.Id);
            IActor copiedTarget = (IActor)copiedCard.GetValidTargets().First();
            copiedCard.PlayCard(copiedTarget);
            Assert.That(copiedContext.GetEnemies().First().Health, Is.EqualTo(99)); //should have only dealt 1 damage
        }

        [Test]
        public void TestAttributes()
        {
        }

        [Test]
        public void TestMultipleCardsDontShareState()
        {
            Card card = FindCardInDeck("DealMoreDamageEachPlay");
            Card copy = Context.CopyCard(card);
            Context.GetCurrentBattle().Deck.HandPile.Cards.Add(copy);
            IActor target = (IActor)card.GetValidTargets().First();
            Assert.That(target.Health, Is.EqualTo(100));
            card.PlayCard(target);
            Assert.That(target.Health, Is.EqualTo(99));

            copy.PlayCard(target);
            Assert.That(target.Health, Is.EqualTo(98));
            copy.PlayCard(target);
            Assert.That(target.Health, Is.EqualTo(96));
        }

        [Test]
        public void TestBattleStateAndPermanentState()
        {
            BattleStateAndPermanentState card =
                (BattleStateAndPermanentState)FindCardInDeck("BattleStateAndPermanentState");
            Assert.That(Context.PlayerDeck.Contains(card));

            IActor target = (IActor)card.GetValidTargets().First();
            Assert.That(target.Health, Is.EqualTo(100));
            card.PlayCard(target);
            Assert.That(target.Health, Is.EqualTo(98));

            //end the battle
            ((IInternalBattleEventHandler)Context.Events).InvokeBattleEnded(this, new BattleEndedEventArgs(true));
            //start new battle
            var player = Context.GetCurrentBattle().Player;
            Context.StartBattle(player, Context.CreateEntity<BasicBattleData>());
            target = (IActor)card.GetValidTargets().First();
            Assert.That(target.Health, Is.EqualTo(100));
            card.PlayCard(target);
            Assert.That(target.Health, Is.EqualTo(97));

            BattleStateAndPermanentState copy = (BattleStateAndPermanentState)Context.CopyCard(card);
            Assert.That(copy.TimesPlayed, Is.EqualTo(card.TimesPlayed));
            Assert.That(copy.TimesPlayedThisCombat, Is.EqualTo(card.TimesPlayedThisCombat));
            Context.GetCurrentBattle().Deck.DrawPile.Cards.Add(copy);
            //next attack should deal 5
            copy.PlayCard(target);
            Assert.That(target.Health, Is.EqualTo(92));

            //this code right here is probably what we need to do to create an out-of-battle context, but like every time the game state dirties and the player is checking their deck.
            GameContext outOfBattleContext = GetCopiedContext();
            ((IInternalBattleEventHandler)outOfBattleContext.Events).InvokeBattleEnded(this,
                new BattleEndedEventArgs(true));
            var outOfBattleCopiedCard =
                outOfBattleContext.GetCurrentBattle().Deck.AllCards().First(c => c.Id == copy.Id);
            Assert.That(outOfBattleCopiedCard.GetCardText(), Is.Not.EqualTo(copy.GetCardText()));

            Assert.That(outOfBattleCopiedCard.GetCardText(),
                Is.EqualTo(
                    "Deal 4 to target enemy. Then deal 1.")); //this indicates that the out-of-battle card had its battleproperty reset to zero.
        }

        [Test]
        public void TestCopiedContextGetsSerializedData()
        {
            Card card = FindCardInDeck("DealMoreDamageEachPlay");
            IActor target = (IActor)card.GetValidTargets().First();
            Assert.That(target.Health, Is.EqualTo(100));
            card.PlayCard(target);
            Assert.That(target.Health, Is.EqualTo(99));

            var copiedContext = GetCopiedContext();
            GameContext.CurrentContext = copiedContext;
            Assert.That(copiedContext.GetEnemies().First().Health, Is.EqualTo(99));

            Card copiedCard = copiedContext.GetCurrentBattle().Deck.AllCards().First(c => c.Id == card.Id);
            Assert.That(copiedCard.Context, Is.Not.Null);
            IActor copiedTarget = (IActor)copiedCard.GetValidTargets().First();
            copiedCard.PlayCard(copiedTarget);
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

    internal class BaseTest
    {
        protected static IContext Context { get; set; }

        protected readonly JsonSerializerSettings m_JsonSerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
            ContractResolver = new PrivateSetterContractResolver(),
            Converters = new List<JsonConverter>
            {
                new GameEntityConverter()
            }
        };

        protected PlayerActor Player { get;  set; }
        
        protected void CreateDeck(IContext context)
        {
            foreach (Card card in CreateCards(context))
            {
                Context.PlayerDeck.Add(card);
            }
        }
        protected Card FindCardInDeck(string name)
        {
            return Context.GetCurrentBattle().Deck.AllCards().First(card => card.Name == name);
        }
        private IEnumerable<Card> CreateCards(IContext context)
        {
            yield return context.CreateEntity<Attack5Damage>();
            yield return context.CreateEntity<DoubleNextCardDamage>();
            yield return context.CreateEntity<Attack10DamageExhaust>();
            yield return context.CreateEntity<BattleStateAndPermanentState>();
            yield return context.CreateEntity<DealMoreDamageEachPlay>();
            yield return context.CreateEntity<PommelStrike>();
            yield return context.CreateEntity<Strike>();
            yield return context.CreateEntity<Defend>();
            yield return context.CreateEntity<MoveToEmptyAdjacentNode>();
        }
    }
}