using Api;

namespace Solitaire
{
    public class StandardDeckCard : Component
    {
        public Suit Suit { get; private set; }
        public int Number { get; private set; }


        public void SetCard(int number, Suit suit)
        {
            Suit = suit;
            Number = number;
        }
    }

    public enum Suit
    {
        Clubs,
        Spades,
        Hearts,
        Diamonds
    }

    public abstract class Pile : Component
    {
        public abstract bool TryAddCard(StandardDeckCard entity);
    }

    public class DeckPile : Pile
    {
        public override bool TryAddCard(StandardDeckCard entity)
        {
            if (Parent.GetComponentInParent<SolitaireGame>().GameStarted)
            {
                return false;
            }
            
            entity.Parent.SetParent(Parent);
            return true;
        }
    }
    
    public class HandPile : Pile
    {
        private int MaxHandSize = 5;
        public override bool TryAddCard(StandardDeckCard entity)
        {
            if (Parent.Children.Count >= MaxHandSize)
            {
                return false;
            }
            
            entity.Parent.SetParent(Parent);
            return true;
        }
    }

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