using System.Linq;
using Api;
using CardsAndPiles;

namespace Solitaire
{
    public class StandardDeckCard : Component, IPileItem
    {
        public Suit Suit { get; private set; }
        public int Number { get; private set; }


        public void SetCard(int number, Suit suit)
        {
            Suit = suit;
            Number = number;
        }

        //Should this just be an extension method?
        public bool TrySendToPile(IPile pile)
        {
            return pile.ReceiveItem(this);
        }
    }

    public enum Suit
    {
        Clubs,
        Spades,
        Hearts,
        Diamonds
    }

    public class SolitaireDeckPileConstraint : Component, IPileConstraint
    {
        public bool CanReceive(Entity itemView)
        {
            return !Parent.GetComponentInParent<SolitaireGame>().GameStarted;
        }
    }

    public abstract class Pile : Component, IPile
    {
        public abstract bool ReceiveItem(IPileItem itemView);

        public void RemoveItem(IPileItem itemView)
        {
        }

    }

    public class DeckPile : Pile
    {
        public override bool ReceiveItem(IPileItem item)
        {
            if ((!(item is StandardDeckCard card)) || Parent.GetComponentInParent<SolitaireGame>().GameStarted)
            {
                return false;
            }

            card.Parent.SetParent(Parent);
            return true;
        }
    }

    public class HandPile : Pile
    {
        private int MaxHandSize = 5;


        public override bool ReceiveItem(IPileItem item)
        {
            if ((!(item is StandardDeckCard card)) || Parent.Children.Count >= MaxHandSize)
            {
                return false;
            }

            card.Parent.SetParent(Parent);
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

    public class SolitairePileConstraint : Component, IPileConstraint
    {
        public bool CanReceive(Entity item)
        {
            var card = item.GetComponent<StandardDeckCard>();

            return card != null && SuitCompatible(card.Suit) && IsNextSequence(card.Number);
        }

        private bool IsNextSequence(int number)
        {
            if (Parent.Children.Last().GetComponent<StandardDeckCard>().Number == number - 1)
            {
                return true;
            }

            return false;
        }

        private bool SuitCompatible(Suit suit)
        {
            if (Parent.Children.Count == 0)
            {
                return true;
            }

            return Parent.Children.First().GetComponent<StandardDeckCard>().Suit == suit;
        }
    }
}