using System.Collections.Generic;
using System.Linq;
using Api;
using CardsAndPiles;

namespace Solitaire
{
    public class SolitaireGame : Component
    {
        public DeckPile Deck { get; private set; }
        public bool GameStarted { get; private set; }
        private int NumPiles => 7;

        public List<StandardDeckCard> Cards { get; } = new List<StandardDeckCard>();

        protected override void Initialize()
        {
            base.Initialize();
            GameStarted = false;

            IEntity deckEntity = Context.CreateEntity(Entity);
            Deck = deckEntity.AddComponent<DeckPile>();


            IEntity handEntity = Context.CreateEntity(Entity);
            handEntity.AddComponent<HandPile>();

            for (int i = 0; i < 13; i++)
            {
                MakeCard(i, Suit.Clubs, deckEntity);
                MakeCard(i, Suit.Hearts, deckEntity);
                MakeCard(i, Suit.Spades, deckEntity);
                MakeCard(i, Suit.Diamonds, deckEntity);
            }

            for (int i = 0; i < NumPiles; i++)
            {
                IEntity bankEntity = Context.CreateEntity(Entity);
                bankEntity.AddComponent<BankPile>();
                bankEntity.TrySetParent(Entity);
            }

            IEntity heartsEntity = Context.CreateEntity(Entity);
            heartsEntity.AddComponent<SolutionPile>().Suit = Suit.Hearts;
            heartsEntity.TrySetParent(Entity);

            IEntity clubsEntity = Context.CreateEntity(Entity);
            clubsEntity.AddComponent<SolutionPile>().Suit = Suit.Clubs;
            clubsEntity.TrySetParent(Entity);

            IEntity diamondsEntity = Context.CreateEntity(Entity);
            diamondsEntity.AddComponent<SolutionPile>().Suit = Suit.Diamonds;
            diamondsEntity.TrySetParent(Entity);

            IEntity spadesEntity = Context.CreateEntity(Entity);
            spadesEntity.AddComponent<SolutionPile>().Suit = Suit.Spades;
            spadesEntity.TrySetParent(Entity);
        }

        private void MakeCard(int num, Suit suit, IEntity parent)
        {
            Context.CreateEntity(parent, entity =>
            {
                StandardDeckCard card = entity.AddComponent<StandardDeckCard>();
                card.SetCard(num, suit);
                Cards.Add(card);
            });
        }

        public void StartGame()
        {
            List<BankPile> banks = Entity.GetComponentsInChildren<BankPile>();

            int i = 2;


            foreach (BankPile bankPile in banks)
            {
                for (int j = 0; j < i; j++)
                {
                    Deck.Entity.Children.First().TrySetParent(bankPile.Entity);
                }

                i++;
            }

            GameStarted = true;
        }
    }
}