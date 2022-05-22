using System;
using Api;
using System.Reflection;

public class Events : Component{
//Found catalog
#region Code for event CardPlayed
    public delegate void CardPlayedEvent (object sender, CardPlayedEventArgs args);
    public class CardPlayedEventArgs {        public  int CardId { get; }
        public  int CardCost { get; }
        public  CardPlayedEventArgs (int CardId, int CardCost   ){
                  this.CardId = CardId; 
              this.CardCost = CardCost; 
}

  }
    public class OnCardPlayedAttribute : EventAttribute {
        public override EventHandle GetEventHandle(MethodInfo attached, object instance, Events events)
        {
            var parameters = attached.GetParameters();
            if (parameters.Length == 0)
            {
                return events.SubscribeToCardPlayed(delegate(object sender, CardPlayedEventArgs args)
                {
                    attached.Invoke(instance, Array.Empty<object>());
                });
            }
            if(parameters[0].ParameterType != typeof(object) ||
            parameters[1].ParameterType != typeof(CardPlayedEventArgs)){
                throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, CardPlayedEventArgs) args)");
            }
            return events.SubscribeToCardPlayed(delegate(object sender, CardPlayedEventArgs args)
            {
                attached.Invoke(instance, new[] { sender, args });
            });
        }

    
    }
        private event Events.CardPlayedEvent CardPlayed;
        internal void OnCardPlayed(Events.CardPlayedEventArgs args)
        {
            CardPlayed?.Invoke(this, args);
        }

        public EventHandle SubscribeToCardPlayed(CardPlayedEvent action)
        {
            CardPlayed += action;
            return new EventHandle(() => CardPlayed -= action);
        } 
#endregion Code for event CardPlayed
#region Code for event CardDiscarded
    public delegate void CardDiscardedEvent (object sender, CardDiscardedEventArgs args);
    public class CardDiscardedEventArgs {        public  int CardId { get; }
        public  int CardCost { get; }
        public  CardDiscardedEventArgs (int CardId, int CardCost   ){
                  this.CardId = CardId; 
              this.CardCost = CardCost; 
}

  }
    public class OnCardDiscardedAttribute : EventAttribute {
        public override EventHandle GetEventHandle(MethodInfo attached, object instance, Events events)
        {
            var parameters = attached.GetParameters();
            if (parameters.Length == 0)
            {
                return events.SubscribeToCardDiscarded(delegate(object sender, CardDiscardedEventArgs args)
                {
                    attached.Invoke(instance, Array.Empty<object>());
                });
            }
            if(parameters[0].ParameterType != typeof(object) ||
            parameters[1].ParameterType != typeof(CardDiscardedEventArgs)){
                throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, CardDiscardedEventArgs) args)");
            }
            return events.SubscribeToCardDiscarded(delegate(object sender, CardDiscardedEventArgs args)
            {
                attached.Invoke(instance, new[] { sender, args });
            });
        }

    
    }
        private event Events.CardDiscardedEvent CardDiscarded;
        internal void OnCardDiscarded(Events.CardDiscardedEventArgs args)
        {
            CardDiscarded?.Invoke(this, args);
        }

        public EventHandle SubscribeToCardDiscarded(CardDiscardedEvent action)
        {
            CardDiscarded += action;
            return new EventHandle(() => CardDiscarded -= action);
        } 
#endregion Code for event CardDiscarded


  }


//TODO: generate attributes with static override functions that get the event handle from a game context.
//TODO: add generation of invoke functions.? 

 