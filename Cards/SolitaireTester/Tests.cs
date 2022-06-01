using System.ComponentModel;
using Api;
using NUnit.Framework;
using Solitaire;

namespace Tests
{
    public class Tests
    {
        private Entity Root;

        [SetUp]
        public void Setup()
        {
            var game = new Entity();

            Root = new Entity();
            Root.AddComponent<SolitaireGame>();
            Root.SetParent(game);
        }

        [Test]
        public void Test1()
        {
            Assert.NotNull(Root.GetComponent<SolitaireGame>());
            var deck = Root.GetComponentInChildren<DeckPile>();
            Assert.NotNull(deck);
            Assert.NotNull(Root.GetComponentInChildren<HandPile>());
            Assert.That(deck.Parent.Children, Has.Count.EqualTo(52));
        }

        [Test]
        public void PropertyChangesGeneratedForSolitaire()
        {
            Assert.NotNull(Root.GetComponent<SolitaireGame>());
            var card = Root.GetComponentInChildren<StandardDeckCard>();
            Assert.NotNull(card);

            bool fired = false;
            card.PropertyChanged += CardOnPropertyChanged;

            void CardOnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                fired = true;
            }

            card.IsFaceDown = true;

            Assert.IsTrue(fired);
        }
    }
}