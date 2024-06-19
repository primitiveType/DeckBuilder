using System.ComponentModel;
using Api;
using CardsAndPiles;
using NUnit.Framework;
using Solitaire;
using DeckPile = Solitaire.DeckPile;

namespace SolitaireTester
{
    public class Tests
    {
        private Context Context { get; set; }
        private SolitaireGame Game { get; set; }

        private IEntity Root => Game.Entity;

        [SetUp]
        public void Setup()
        {
            Context = new Context(new CardEvents());
            IEntity gameEntity = Context.CreateEntity();
            Game = gameEntity.AddComponent<SolitaireGame>();
        }

        [Test]
        public void Test1()
        {
            Assert.That(Root.GetComponent<SolitaireGame>(), Is.Not.Null);
            DeckPile deck = Root.GetComponentInChildren<DeckPile>();
            Assert.That(deck, Is.Not.Null);
            Assert.That(Root.GetComponentInChildren<HandPile>(), Is.Not.Null);
            Assert.That(deck.Entity.Children, Has.Count.EqualTo(52));
        }

        [Test]
        public void PropertyChangesGeneratedForSolitaire()
        {
            Assert.That(Root.GetComponent<SolitaireGame>(), Is.Not.Null);
            StandardDeckCard card = Root.GetComponentInChildren<StandardDeckCard>();
            Assert.That(card, Is.Not.Null);

            bool fired = false;
            card.PropertyChanged += CardOnPropertyChanged;

            void CardOnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                fired = true;
            }

            card.IsFaceDown = true;

            Assert.That(fired, Is.True);
        }
    }
}