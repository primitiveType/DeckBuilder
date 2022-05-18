public class Events
{
//Found catalog
//Found event 
    public delegate void CardPlayedEvent(object sender, CardPlayedEventArgs args);

    public class CardPlayedEventArgs
    {
        public int CardId { get; }
        public int CardCost { get; }

        public CardPlayedEventArgs(int CardId, int CardCost)
        {
            this.CardId = CardId;
            this.CardCost = CardCost;
        }
    }

    public class OnCardPlayedAttribute : Attribute
    {
    }

//Found event 
    public delegate void CardDiscardedEvent(object sender, CardDiscardedEventArgs args);

    public class CardDiscardedEventArgs
    {
        public int CardId { get; }
        public int CardCost { get; }

        public CardDiscardedEventArgs(int CardId, int CardCost)
        {
            this.CardId = CardId;
            this.CardCost = CardCost;
        }
    }

    public class OnCardDiscardedAttribute : Attribute
    {
    }
}


//TODO: generate attributes with static override functions that get the event handle from a game context.
//TODO: add generation of invoke functions.? 