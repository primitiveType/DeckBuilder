using System.Collections.Generic;
using System.Linq;
using Data;
using NUnit.Framework;

[TestFixture]
internal class ApiTests
{
    private GlobalApi Api;

    [SetUp]
    public void Setup()
    {
        Api = new GlobalApi();
        Actor player = new Actor(100);
        Actor enemy = new Actor(100);

        Deck deck = CreateDeck();

        Battle battle = new Battle(player, new List<Actor> {enemy}, deck);

        Api.SetCurrentBattle(battle);
    }

    private Deck CreateDeck()
    {
        Deck deck = new Deck();
        foreach (var card in CreateCards())
        {
            deck.DrawPile.Add(card);
        }

        return deck;
    }

    private IEnumerable<Card> CreateCards()
    {
        yield return CreateCard(TestCards.Attack5Damage, nameof(TestCards.Attack5Damage));
        yield return CreateCard(TestCards.Attack10DamageExhaust, nameof(TestCards.Attack10DamageExhaust));
    }

    [Test]
    public void TestThatSetupWorks()
    {
        //Check that setup is working
        Assert.That(Api.GetPlayerHealth(), Is.EqualTo(100));
        IReadOnlyList<Actor> enemies = Api.GetEnemies();
        Assert.That(enemies, Has.Count.EqualTo(1));
    }


    [Test]
    public void TestGetValidTargets()
    {
        Card card = Api.CreateCardInstance(nameof(TestCards.Attack5Damage));
        List<Actor> targets = card.GetValidTargets();

        Assert.That(targets, Has.Count.EqualTo(Api.GetEnemies().Count));
        Assert.That(targets, Has.Count.EqualTo(1));
        Actor firstTarget = targets[0];
        Actor firstEnemy = Api.GetEnemies()[0];
        Assert.That(firstTarget.Armor, Is.EqualTo(firstEnemy.Armor));
        Assert.That(firstTarget.Health, Is.EqualTo(firstEnemy.Health));
        Assert.That(firstTarget.Id, Is.EqualTo(firstEnemy.Id));
        Assert.That(firstTarget, Is.EqualTo(firstEnemy));
    }

    private Card CreateCard(string cardScript, string cardName)
    {
        ((IGlobalApi) Api).AddCard(cardScript, cardName);
        Card card = Api.CreateCardInstance(cardName);
        return card;
    }

    [Test]
    public void TestAttackDealsDamage()
    {
        Card card = Api.GetCurrentBattle().Deck.DrawPile.Find(draw => draw.Name == nameof(TestCards.Attack5Damage));
        var targets = card.GetValidTargets();
        Assert.That(targets[0].Health, Is.EqualTo(100));
        card.PlayCard(targets[0]);
        Assert.That(targets[0].Health, Is.LessThan(100));
    }


    [Test]
    public void TestCardIsDiscarded()
    {
        bool receivedMoveEvent = false;
        Card cardToPlay = Api.GetCurrentBattle().Deck.DrawPile.First(card => card.Name == nameof(TestCards.Attack5Damage));
        Actor target = cardToPlay.GetValidTargets()[0];
        Api.GetCurrentBattle().Deck.CardMoved += DeckOnOnCardMoved;
        cardToPlay.PlayCard(target);
        Assert.That(Api.GetCurrentBattle().Deck.DrawPile, !Contains.Item(cardToPlay));
        Assert.That(Api.GetCurrentBattle().Deck.DiscardPile, Contains.Item(cardToPlay));
        if (!receivedMoveEvent)
        {
            Assert.Fail("Never received event for card move!");
        }

        void DeckOnOnCardMoved(object sender, CardMovedEventArgs args)
        {
            receivedMoveEvent = true;
            Assert.That(args.MovedCard, Is.EqualTo(cardToPlay));
            Assert.That(args.NewPile, Is.EqualTo(CardPile.DiscardPile));
            Assert.That(args.PreviousPile, Is.EqualTo(CardPile.DrawPile));
        }
    }


    [Test]
    public void TestCardIsExhausted()
    {
        bool receivedMoveEvent = false;
        Card cardToPlay = Api.GetCurrentBattle().Deck.DrawPile.First(card => card.Name == nameof(TestCards.Attack10DamageExhaust));
        Actor target = cardToPlay.GetValidTargets()[0];
        Api.GetCurrentBattle().Deck.CardMoved += DeckOnCardMoved;
        cardToPlay.PlayCard(target);
        Assert.That(Api.GetCurrentBattle().Deck.DrawPile, !Contains.Item(cardToPlay));
        Assert.That(Api.GetCurrentBattle().Deck.ExhaustPile, Contains.Item(cardToPlay));
        if (!receivedMoveEvent)
        {
            Assert.Fail("Never received event for card move!");
        }

        void DeckOnCardMoved(object sender, CardMovedEventArgs args)
        {
            receivedMoveEvent = true;
            Assert.That(args.MovedCard, Is.EqualTo(cardToPlay));
            Assert.That(args.NewPile, Is.EqualTo(CardPile.ExhaustPile));
            Assert.That(args.PreviousPile, Is.EqualTo(CardPile.DrawPile));
        }
    }

    [Test]
    public void TestCardIsDuplicated()
    {
        Assert.Fail();
    }

    [Test]
    public void TestDuplicatedCardHasCorrectState()
    {
        Assert.Fail();
    }
}

public static class TestCards
{
    //Declaring these test cards here, so it doesn't show up in game, or randomly change, breaking tests.
    //They might need to be updated if changes to the api happen though.
    public const string Attack5Damage =
        @"function getValidTargets ()
            return GetEnemyIds()--Basic attack that considers any enemy a valid target 
          end

          function playCard(cardId, target)
            DamageTarget(target, 5)
          end

           function onDamageDealt(target, totalDamage, healthDamage)
            Log('dealt ' .. totalDamage .. ' damage.')
            end
          
            function onCardPlayed (cardId)
                SendToDiscard(cardId)
            end
";
    
    public const string Attack10DamageExhaust =
        @"function getValidTargets ()
            return GetEnemyIds()--Basic attack that considers any enemy a valid target 
          end

          function playCard(cardId, target)
            DamageTarget(target, 10)
          end

           function onDamageDealt(target, totalDamage, healthDamage)
            Log('dealt ' .. totalDamage .. ' damage.')
            end
          
            function onCardPlayed (cardId)
                SendToExhaust(cardId)
            end
";
}