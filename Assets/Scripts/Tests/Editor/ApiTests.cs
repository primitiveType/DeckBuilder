﻿using System.Collections.Generic;
using System.Linq;
using Data;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;

[TestFixture]
internal class ApiTests
{
    //I'd really like to abstract this away with an injector or something... should be possible to do all simulation
    //without any unity objects.
    private IGlobalApi Api => Injector.GlobalApi;

    [SetUp]
    public void Setup()
    {
        Injector.Initialize();
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
        yield return CreateCard(TestCards.DealMoreDamageEachPlay, nameof(TestCards.DealMoreDamageEachPlay));
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
        Card card = FindCardInDeck(nameof(TestCards.Attack5Damage));
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
        Card card = FindCardInDeck(nameof(TestCards.Attack5Damage));
        var targets = card.GetValidTargets();
        Assert.That(targets[0].Health, Is.EqualTo(100));
        card.PlayCard(targets[0]);
        Assert.That(targets[0].Health, Is.LessThan(100));
    }


    [Test]
    public void TestAttackCausesEvent()
    {
        bool gotEvent = false;
        Card card = FindCardInDeck(nameof(TestCards.Attack5Damage));
        List<Actor> targets = card.GetValidTargets();
        Injector.GameEventHandler.DamageDealt += GameEventHandlerOnDamageDealt;
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
        Card cardToPlay = FindCardInDeck(nameof(TestCards.Attack5Damage));
        Actor target = cardToPlay.GetValidTargets()[0];
        Injector.GameEventHandler.CardMoved += DeckOnOnCardMoved;
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
            Assert.That(args.MovedCard, Is.EqualTo(cardToPlay.Id));
            Assert.That(args.NewPile, Is.EqualTo(CardPile.DiscardPile));
            Assert.That(args.PreviousPile, Is.EqualTo(CardPile.DrawPile));
        }
    }


    [Test]
    public void TestCardIsExhausted()
    {
        bool receivedMoveEvent = false;
        Card cardToPlay = FindCardInDeck(nameof(TestCards.Attack10DamageExhaust));
        Actor target = cardToPlay.GetValidTargets()[0];
        Injector.GameEventHandler.CardMoved += DeckOnCardMoved;
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
            Assert.That(args.MovedCard, Is.EqualTo(cardToPlay.Id));
            Assert.That(args.NewPile, Is.EqualTo(CardPile.ExhaustPile));
            Assert.That(args.PreviousPile, Is.EqualTo(CardPile.DrawPile));
        }
    }

    private Card FindCardInDeck(string name)
    {
        return Api.GetCurrentBattle().Deck.AllCards().First(card => card.Name == name);
    }

    private Card FindCardInDeck(int cardId)
    {
        return Api.GetCurrentBattle().Deck.AllCards().First(card => card.Id == cardId);
    }

    [Test]
    public void TestCardStatefulness()
    {
        Card card = FindCardInDeck(nameof(TestCards.DealMoreDamageEachPlay));
        Card dup = Api.CreateCardInstance(nameof(TestCards
            .DealMoreDamageEachPlay)); //make sure there are two of the same card
        Api.GetCurrentBattle().Deck.DrawPile.Add(dup);

        List<Actor> targets = card.GetValidTargets();
        Assert.That(targets[0].Health, Is.EqualTo(100));
        card.PlayCard(targets[0]);
        Assert.That(targets[0].Health, Is.EqualTo(99));
        card.PlayCard(targets[0]);
        Assert.That(targets[0].Health, Is.EqualTo(97)); //deals more damage each time.

        dup.PlayCard(targets[0]);
        Assert.That(targets[0].Health, Is.EqualTo(96)); //the dup has not been played yet, so it only deals 1 damage.

        Card copy = card.Duplicate();
        Api.GetCurrentBattle().Deck.HandPile.Add(copy);
        copy.PlayCard(targets[0]);
        Assert.That(targets[0].Health,
            Is.EqualTo(93)); //the copy should deal 3 damage because it retains the state of its progenitor.
    }

    [Test]
    public void TestCardSaving()
    {
        Card card = FindCardInDeck(nameof(TestCards.DealMoreDamageEachPlay));

        List<Actor> targets = card.GetValidTargets();
        Assert.That(targets[0].Health, Is.EqualTo(100));
        card.PlayCard(targets[0]);
        Assert.That(targets[0].Health, Is.EqualTo(99));
        card.PlayCard(targets[0]);
        Assert.That(targets[0].Health, Is.EqualTo(97)); //deals more damage each time.

        string cardStr = JsonConvert.SerializeObject(card);

        var cardCopy = Api.LoadCardFromJson(cardStr);
        Api.GetCurrentBattle().Deck.DrawPile.Add(cardCopy);
        Debug.Log(cardStr);
        Assert.That(cardStr, Contains.Substring("Name"));

        cardCopy.PlayCard(targets[0]);
        Assert.That(targets[0].Health, Is.EqualTo(94)); //deals more damage each time, even when re-loading the game.
    }

    private const string SavedCard = "{\"Name\":\"DealMoreDamageEachPlay\",\"SaveData\":3,\"Id\":88241230}";

    [Test]
    public void TestCardLoading()
    {
        var cardCopy = Api.LoadCardFromJson(SavedCard);
        Api.GetCurrentBattle().Deck.DrawPile.Add(cardCopy);
        List<Actor> targets = cardCopy.GetValidTargets();

        cardCopy.PlayCard(targets[0]);
        Assert.That(targets[0].Health, Is.EqualTo(97)); //deals more damage each time.
    }
}

