using System.Collections.Generic;
using System.Linq;
using Content.Cards;
using Content.Cards.TestCards;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using JsonNet.ContractResolvers;
using Newtonsoft.Json;

namespace DeckbuilderTests
{
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
                new GameEntityConverter(),
                new StringToHexConverter()
            }
        };
        protected PlayerActor Player { get; set; }

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
            yield return context.CreateEntity<TestAttack5Damage>();
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