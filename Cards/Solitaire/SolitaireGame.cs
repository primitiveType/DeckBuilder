using System.Collections.Generic;
using System.Linq;
using Api;
using RandN;

namespace Solitaire
{
    public class SolitaireGame : Component
    {
        public DeckPile Deck { get; private set; }
        public bool GameStarted { get; private set; }
        private int NumPiles => 7;

        private List<StandardDeckCard> Cards = new List<StandardDeckCard>();

        protected override void Initialize()
        {
            base.Initialize();
            GameStarted = false;
            var random = Parent.AddComponent<Random>();

            Entity deckEntity = new Entity();
            Deck = deckEntity.AddComponent<DeckPile>();
            deckEntity.SetParent(Parent);

            Deck.Shuffle();

            Entity handEntity = new Entity();
            handEntity.AddComponent<HandPile>();
            handEntity.SetParent(Parent);

            for (int i = 0; i < 13; i++)
            {
                MakeCard(i, Suit.Clubs).SetParent(deckEntity);
                MakeCard(i, Suit.Hearts).SetParent(deckEntity);
                MakeCard(i, Suit.Spades).SetParent(deckEntity);
                MakeCard(i, Suit.Diamonds).SetParent(deckEntity);
            }

            for (int i = 0; i < NumPiles; i++)
            {
                Entity bankEntity = new Entity();
                bankEntity.AddComponent<BankPile>();
                bankEntity.SetParent(Parent);
            }

            Entity heartsEntity = new Entity();
            heartsEntity.AddComponent<SolutionPile>().Suit = Suit.Hearts;
            heartsEntity.SetParent(Parent);

            Entity clubsEntity = new Entity();
            clubsEntity.AddComponent<SolutionPile>().Suit = Suit.Clubs;
            clubsEntity.SetParent(Parent);

            Entity diamondsEntity = new Entity();
            diamondsEntity.AddComponent<SolutionPile>().Suit = Suit.Diamonds;
            diamondsEntity.SetParent(Parent);

            Entity spadesEntity = new Entity();
            spadesEntity.AddComponent<SolutionPile>().Suit = Suit.Spades;
            spadesEntity.SetParent(Parent);
        }

        private Entity MakeCard(int num, Suit suit)
        {
            Entity cardEntity = new Entity();
            StandardDeckCard card = cardEntity.AddComponent<StandardDeckCard>();
            card.SetCard(num, suit);

            Cards.Add(card);
            return cardEntity;
        }

        public void StartGame()
        {
            List<BankPile> banks = Parent.GetComponentsInChildren<BankPile>();

            int i = 2;


            foreach (BankPile bankPile in banks)
            {
                for (int j = 0; j < i; j++)
                {
                    Deck.Parent.Children.First().SetParent(bankPile.Parent);
                }

                i++;
            }

            GameStarted = true;
        }
    }
}