using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

[TestFixture]
internal class ApiTests
{
    //I'd really like to abstract this away with an injector or something... should be possible to do all simulation
    //without any unity objects.
    private IGlobalApi Api => Injector.GlobalApi;
    [UnitySetUp]
    public IEnumerator Setup()
    {
        Injector.Initialize();
        Actor player = new Actor(100);
        Actor enemy = new Actor(100);

        Deck deck = CreateDeck();

        Battle battle = new Battle(player, new List<Actor> {enemy}, deck);

        Api.SetCurrentBattle(battle);
        yield return null;
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

    [UnityTest]
    public IEnumerator TestThatSetupWorks()
    {
        //Check that setup is working
        Assert.That(Api.GetPlayerHealth(), Is.EqualTo(100));
        IReadOnlyList<Actor> enemies = Api.GetEnemies();
        Assert.That(enemies, Has.Count.EqualTo(1));
        yield return null;
    }


    [UnityTest]
    public IEnumerator TestGetValidTargets()
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
        yield return null;
    }

    private Card CreateCard(string cardScript, string cardName)
    {
        ((IGlobalApi) Api).AddCard(cardScript, cardName);
        Card card = Api.CreateCardInstance(cardName);
        return card;
    }

    [UnityTest]
    public IEnumerator TestAttackDealsDamage()
    {
        Card card = FindCardInDeck(nameof(TestCards.Attack5Damage));
        var targets = card.GetValidTargets();
        Assert.That(targets[0].Health, Is.EqualTo(100));
        card.PlayCard(targets[0]);
        Assert.That(targets[0].Health, Is.LessThan(100));
        yield return null;
    }


    [UnityTest]
    public IEnumerator TestCardIsDiscarded()
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
            Assert.That(args.MovedCard, Is.EqualTo(cardToPlay));
            Assert.That(args.NewPile, Is.EqualTo(CardPile.DiscardPile));
            Assert.That(args.PreviousPile, Is.EqualTo(CardPile.DrawPile));
        }
        yield return null;
    }


    [UnityTest]
    public IEnumerator TestCardIsExhausted()
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
            Assert.That(args.MovedCard, Is.EqualTo(cardToPlay));
            Assert.That(args.NewPile, Is.EqualTo(CardPile.ExhaustPile));
            Assert.That(args.PreviousPile, Is.EqualTo(CardPile.DrawPile));
        }
        yield return null;
    }

    private Card FindCardInDeck(string name)
    {
        return Api.GetCurrentBattle().Deck.AllCards().First(card => card.Name == name);
    }

    private Card FindCardInDeck(int cardId)
    {
        return Api.GetCurrentBattle().Deck.AllCards().First(card => card.Id == cardId);
    }

    [UnityTest]
    public IEnumerator TestCardStatefulness()
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
        yield return null;
        yield return null;
    }

    [UnityTest]
    public IEnumerator TestCardSaving()
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
        yield return null;
    }

    private const string SavedCard = "{\"Name\":\"DealMoreDamageEachPlay\",\"SaveData\":3,\"Id\":88241230}";

    [UnityTest]
    public IEnumerator TestCardLoading()
    {
        var cardCopy = Api.LoadCardFromJson(SavedCard);
        Api.GetCurrentBattle().Deck.DrawPile.Add(cardCopy);
        List<Actor> targets = cardCopy.GetValidTargets();

        cardCopy.PlayCard(targets[0]);
        Assert.That(targets[0].Health, Is.EqualTo(97)); //deals more damage each time.
        yield return null;
    }
}

public static class TestCards
{
    //If a function doesn't exist on the lua side it will throw errors when we attempt to call it.
    //So we declare all functions in a template and just append the scripts to it. We will do something similar for real cards.
    //re-declaring a function in lua is legal and just overrides it.
    //This also serves as a nice one-stop location to see what calls a lua script can implement.
    private const string BaseCardTemplate =
        @"function getValidTargets(cardId) end
          function playCard(cardId, target) end
          function onDamageDealt(cardId, target, totalDamage, healthDamage) end
          function log(cardId) end
          function onCardPlayed(cardId) end
          function cardInstanceCreated(cardId) end
          function getCardData(cardId) end
          ";

    //Declaring these test cards here, so it doesn't show up in game, or randomly change, breaking tests.
    //They might need to be updated if changes to the api happen though.
    public const string Attack5Damage = BaseCardTemplate +
                                        @"function getValidTargets (cardId)
                                            return GetEnemyIds()--Basic attack that considers any enemy a valid target 
                                          end

                                          function playCard(cardId, target)
                                            DamageTarget(target, 5)
                                          end

                                           function onDamageDealt(cardId, target, totalDamage, healthDamage)
                                            Log('dealt ' .. totalDamage .. ' damage.')
                                            end
                                          
                                            function onCardPlayed (cardId)
                                                SendToDiscard(cardId)
                                            end
                                ";

    public const string Attack10DamageExhaust = BaseCardTemplate +
                                                @"function getValidTargets (cardId)
                                                    return GetEnemyIds()--Basic attack that considers any enemy a valid target 
                                                  end

                                                  function playCard(cardId, target)
                                                    DamageTarget(target, 10)
                                                  end

                                                   function onDamageDealt(cardId, target, totalDamage, healthDamage)
                                                    Log('dealt ' .. totalDamage .. ' damage.')
                                                    end
                                                  
                                                    function onCardPlayed (cardId)
                                                        SendToExhaust(cardId)
                                                    end
                                        ";

    public const string DealMoreDamageEachPlay = BaseCardTemplate +
                                                 @"
                                                  damageAmount = {}

                                                    function cardInstanceCreated(cardId, opaque)
                                                        if opaque == nil then
                                                            damageAmount[cardId] = 1
                                                        else
                                                            damageAmount[cardId] = opaque
                                                        end
                                                    end

                                                    
                                                  function getCardData(cardId)
                                                    return damageAmount[cardId]
                                                  end
                                          
                                                  function getValidTargets (cardId)
                                                    return GetEnemyIds()--Basic attack that considers any enemy a valid target 
                                                  end

                                                  function playCard(cardId, target)
                                                    DamageTarget(target, damageAmount[cardId])
                                                    damageAmount[cardId] = damageAmount[cardId] + 1;
                                                  end

                                                   function onDamageDealt(cardId, target, totalDamage, healthDamage)
                                                    Log('dealt ' .. totalDamage .. ' damage.')
                                                    end
                                                  
                                                    function onCardPlayed (cardId)
                                                        SendToExhaust(cardId)
                                                    end

                                                    
                                        ";
}