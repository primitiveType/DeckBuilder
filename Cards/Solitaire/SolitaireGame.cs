using Api;

namespace Solitaire
{
    public class SolitaireGame : Component
    {
        public bool GameStarted { get; private set; }

        protected override void Initialize()
        {
            base.Initialize();
            GameStarted = false;
            Entity deckEntity = new Entity();
            deckEntity.AddComponent<DeckPile>();
            deckEntity.SetParent(Parent);

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
        }

        private Entity MakeCard(int num, Suit suit)
        {
            var cardEntity = new Entity();
            var card = cardEntity.AddComponent<StandardDeckCard>();
            card.SetCard(num, suit);

            return cardEntity;
        }
    }
}