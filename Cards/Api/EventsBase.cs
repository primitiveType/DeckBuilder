using System;
using Api;
using System.Reflection;
using System.Collections.Generic;

public abstract class EventsBase : Component{
    #region Code for event CardPlayed
private event CardPlayedEvent CardPlayed;
internal virtual void OnCardPlayed(CardPlayedEventArgs args)
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
private event CardDiscardedEvent CardDiscarded;
internal virtual void OnCardDiscarded(CardDiscardedEventArgs args)
{
    CardDiscarded?.Invoke(this, args);
}

public EventHandle SubscribeToCardDiscarded(CardDiscardedEvent action)
{
    CardDiscarded += action;
    return new EventHandle(() => CardDiscarded -= action);
} 
    #endregion Code for event CardDiscarded
    #region Code for event RequestDealDamage
private event RequestDealDamageEvent RequestDealDamage;
internal virtual void OnRequestDealDamage(RequestDealDamageEventArgs args)
{
    RequestDealDamage?.Invoke(this, args);
}

public EventHandle SubscribeToRequestDealDamage(RequestDealDamageEvent action)
{
    RequestDealDamage += action;
    return new EventHandle(() => RequestDealDamage -= action);
} 
    #endregion Code for event RequestDealDamage
}
/// <summary>
/// (object sender, CardPlayedEventArgs) args)
/// </summary>
public class OnCardPlayedAttribute : EventAttribute {
    public override EventHandle GetEventHandle(MethodInfo attached, object instance, EventsBase events)
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
    public delegate void CardPlayedEvent (object sender, CardPlayedEventArgs args);

    public class CardPlayedEventArgs {        public  int CardId { get; }
        public  int CardCost { get; }
        public  CardPlayedEventArgs (int CardId, int CardCost   ){
                  this.CardId = CardId; 
              this.CardCost = CardCost; 
}

  }
/// <summary>
/// (object sender, CardDiscardedEventArgs) args)
/// </summary>
public class OnCardDiscardedAttribute : EventAttribute {
    public override EventHandle GetEventHandle(MethodInfo attached, object instance, EventsBase events)
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
    public delegate void CardDiscardedEvent (object sender, CardDiscardedEventArgs args);

    public class CardDiscardedEventArgs {        public  int CardId { get; }
        public  int CardCost { get; }
        public  CardDiscardedEventArgs (int CardId, int CardCost   ){
                  this.CardId = CardId; 
              this.CardCost = CardCost; 
}

  }
/// <summary>
/// (object sender, RequestDealDamageEventArgs) args)
/// </summary>
public class OnRequestDealDamageAttribute : EventAttribute {
    public override EventHandle GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return events.SubscribeToRequestDealDamage(delegate(object sender, RequestDealDamageEventArgs args)
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(RequestDealDamageEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, RequestDealDamageEventArgs) args)");
        }
        return events.SubscribeToRequestDealDamage(delegate(object sender, RequestDealDamageEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    public delegate void RequestDealDamageEvent (object sender, RequestDealDamageEventArgs args);

    public class RequestDealDamageEventArgs {        public  int Amount { get; }
        public  Entity Source { get; }
        public  Entity Target { get; }
        public  List<float> Multiplier { get; set;} 
=new List<float>();        public  RequestDealDamageEventArgs (int Amount, Entity Source, Entity Target   ){
                  this.Amount = Amount; 
              this.Source = Source; 
              this.Target = Target; 
}

  }



//TODO: generate attributes with static override functions that get the event handle from a game context.
//TODO: add generation of invoke functions.? 

 