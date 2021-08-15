﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using Data;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using UnityEngine;

[TestFixture]
internal class ApiTests
{
    //Lua is stateless
    //State is stored on c# side in relevant objects
    //Basically nothing is static
    //Entire game context can be duplicated and simulated separately
    //


    private static ITestContext Context { get; set; }

    [SetUp]
    public void Setup()
    {
        Context = new GameContext();
        Actor player = new Actor(100, Context);
        Actor enemy = new Actor(100, Context);

        Deck deck = CreateDeck(Context);

        Battle battle = new Battle(player, new List<Actor> {enemy}, deck, Context);

        Context.SetCurrentBattle(battle);
    }


    private Deck CreateDeck(ITestContext context)
    {
        Deck deck = new Deck(context);
        foreach (string card in CreateCards(context))
        {
            deck.DrawPile.Cards.Add(context.CreateCardInstance(card));
        }

        return deck;
    }

    private IEnumerable<string> CreateCards(ITestContext context)
    {
        yield return "Attack5Damage";
        yield return "Attack10DamageExhaust";
        yield return "DealMoreDamageEachPlay";
    }

    [Test]
    public void TestThatSetupWorks()
    {
        //Check that setup is working
        Assert.That(Context.GetPlayerHealth(), Is.EqualTo(100));
        IReadOnlyList<Actor> enemies = Context.GetEnemies();
        Assert.That(enemies, Has.Count.EqualTo(1));
    }


    [Test]
    public void TestGetValidTargets()
    {
        Card card = FindCardInDeck("Attack5Damage");
        List<Actor> targets = card.GetValidTargets();

        Assert.That(targets, Has.Count.EqualTo(Context.GetEnemies().Count));
        Assert.That(targets, Has.Count.EqualTo(1));
        Actor firstTarget = targets[0];
        Actor firstEnemy = Context.GetEnemies()[0];
        Assert.That(firstTarget.Armor, Is.EqualTo(firstEnemy.Armor));
        Assert.That(firstTarget.Health, Is.EqualTo(firstEnemy.Health));
        Assert.That(firstTarget.Id, Is.EqualTo(firstEnemy.Id));
        Assert.That(firstTarget, Is.EqualTo(firstEnemy));
    }

    private Card CreateCard(ITestContext context, string cardScript, string cardName)
    {
        context.AddCard(cardScript, cardName);
        Card card = context.CreateCardInstance(cardName);
        return card;
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
        List<Actor> targets = card.GetValidTargets();
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
        Actor target = cardToPlay.GetValidTargets()[0];
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
        Actor target = cardToPlay.GetValidTargets()[0];
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
    public void TestCardStatefulness()
    {
        Card card = FindCardInDeck("DealMoreDamageEachPlay");
        Card sibling = Context.CreateCardInstance("DealMoreDamageEachPlay"); //make sure there are two of the same card
        Context.GetCurrentBattle().Deck.DrawPile.Cards.Add(sibling);

        List<Actor> targets = card.GetValidTargets();
        Assert.That(targets[0].Health, Is.EqualTo(100));
        card.PlayCard(targets[0]);
        Assert.That(targets[0].Health, Is.EqualTo(99));
        card.PlayCard(targets[0]);
        Assert.That(targets[0].Health, Is.EqualTo(97)); //deals more damage each time.

        sibling.PlayCard(targets[0]);
        Assert.That(targets[0].Health, Is.EqualTo(96)); //the dup has not been played yet, so it only deals 1 damage.

        Card copy = new Card(card);
        Context.GetCurrentBattle().Deck.HandPile.Cards.Add(copy);
        copy.PlayCard(targets[0]);
        Assert.That(targets[0].Health,
            Is.EqualTo(93)); //the copy should deal 3 damage because it retains the state of its progenitor.
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
        List<Actor> targets = card.GetValidTargets();
        Assert.That(targets[0].Health, Is.EqualTo(100));
        card.PlayCard(targets[0]);
        Assert.That(targets[0].Health, Is.EqualTo(99));

        Assert.That(copiedContext.GetEnemies().First().Health, Is.EqualTo(100));
        Card copiedCard = copiedContext.GetCurrentBattle().Deck.AllCards().First(c => c.Id == card.Id);
        copiedCard.PlayCard(targets[0]); //it only uses the id so this actually works... for now.
        Assert.That(copiedContext.GetEnemies().First().Health, Is.EqualTo(99));//should have only dealt 1 damage

    }
    
    [Test]
    public void TestCopiedContextGetsSerializedData()
    {
        Card card = FindCardInDeck("DealMoreDamageEachPlay");
        List<Actor> targets = card.GetValidTargets();
        Assert.That(targets[0].Health, Is.EqualTo(100));
        card.PlayCard(targets[0]);
        Assert.That(targets[0].Health, Is.EqualTo(99));

        var copiedContext = GetCopiedContext();

        Assert.That(copiedContext.GetEnemies().First().Health, Is.EqualTo(99));
        Card copiedCard = copiedContext.GetCurrentBattle().Deck.AllCards().First(c => c.Id == card.Id);
        Assert.That(copiedCard.Properties.Ints["TimesPlayed"], Is.EqualTo(1));
        copiedCard.PlayCard(targets[0]); //it only uses the id so this actually works... for now.
        Assert.That(copiedContext.GetEnemies().First().Health, Is.EqualTo(97));//context copied after the first play so it should deal 2 damage.
        
        Assert.That(card.Properties.Ints["TimesPlayed"], Is.EqualTo(1));

    }


    private GameContext GetCopiedContext()
    {
        string contextStr = JsonConvert.SerializeObject(Context);
        GameContext copy = JsonConvert.DeserializeObject<GameContext>(contextStr);
        return copy;
    }
}