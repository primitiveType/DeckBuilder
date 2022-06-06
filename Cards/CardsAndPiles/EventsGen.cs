
// ReSharper disable RedundantUsingDirective
// ReSharper disable PossibleNullReferenceException
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantCast
using System;
using System.Collections.Generic;
using System.Reflection;
using Api;


namespace CardsAndPiles{
public abstract class CardEventsBase : EventsBase{
    #region Code for event CardPlayed
private event EventHandleDelegate<CardPlayedEventArgs> CardPlayed;
public virtual void OnCardPlayed(CardPlayedEventArgs args)
{
    CardPlayed?.Invoke(this, args);
}

public EventHandle<CardPlayedEventArgs> SubscribeToCardPlayed(EventHandleDelegate<CardPlayedEventArgs> action)
{
    var handler = new EventHandle<CardPlayedEventArgs>(action, () => CardPlayed -= action);
    CardPlayed += handler.Invoke;
    return handler;
} 
    #endregion Code for event CardPlayed
    #region Code for event CardDiscarded
private event EventHandleDelegate<CardDiscardedEventArgs> CardDiscarded;
public virtual void OnCardDiscarded(CardDiscardedEventArgs args)
{
    CardDiscarded?.Invoke(this, args);
}

public EventHandle<CardDiscardedEventArgs> SubscribeToCardDiscarded(EventHandleDelegate<CardDiscardedEventArgs> action)
{
    var handler = new EventHandle<CardDiscardedEventArgs>(action, () => CardDiscarded -= action);
    CardDiscarded += handler.Invoke;
    return handler;
} 
    #endregion Code for event CardDiscarded
    #region Code for event RequestDealDamage
private event EventHandleDelegate<RequestDealDamageEventArgs> RequestDealDamage;
public virtual void OnRequestDealDamage(RequestDealDamageEventArgs args)
{
    RequestDealDamage?.Invoke(this, args);
}

public EventHandle<RequestDealDamageEventArgs> SubscribeToRequestDealDamage(EventHandleDelegate<RequestDealDamageEventArgs> action)
{
    var handler = new EventHandle<RequestDealDamageEventArgs>(action, () => RequestDealDamage -= action);
    RequestDealDamage += handler.Invoke;
    return handler;
} 
    #endregion Code for event RequestDealDamage
    #region Code for event EntityKilled
private event EventHandleDelegate<EntityKilledEventArgs> EntityKilled;
public virtual void OnEntityKilled(EntityKilledEventArgs args)
{
    EntityKilled?.Invoke(this, args);
}

public EventHandle<EntityKilledEventArgs> SubscribeToEntityKilled(EventHandleDelegate<EntityKilledEventArgs> action)
{
    var handler = new EventHandle<EntityKilledEventArgs>(action, () => EntityKilled -= action);
    EntityKilled += handler.Invoke;
    return handler;
} 
    #endregion Code for event EntityKilled
    #region Code for event DamageDealt
private event EventHandleDelegate<DamageDealtEventArgs> DamageDealt;
public virtual void OnDamageDealt(DamageDealtEventArgs args)
{
    DamageDealt?.Invoke(this, args);
}

public EventHandle<DamageDealtEventArgs> SubscribeToDamageDealt(EventHandleDelegate<DamageDealtEventArgs> action)
{
    var handler = new EventHandle<DamageDealtEventArgs>(action, () => DamageDealt -= action);
    DamageDealt += handler.Invoke;
    return handler;
} 
    #endregion Code for event DamageDealt
    #region Code for event TurnEnded
private event EventHandleDelegate<TurnEndedEventArgs> TurnEnded;
public virtual void OnTurnEnded(TurnEndedEventArgs args)
{
    TurnEnded?.Invoke(this, args);
}

public EventHandle<TurnEndedEventArgs> SubscribeToTurnEnded(EventHandleDelegate<TurnEndedEventArgs> action)
{
    var handler = new EventHandle<TurnEndedEventArgs>(action, () => TurnEnded -= action);
    TurnEnded += handler.Invoke;
    return handler;
} 
    #endregion Code for event TurnEnded
    #region Code for event TurnBegan
private event EventHandleDelegate<TurnBeganEventArgs> TurnBegan;
public virtual void OnTurnBegan(TurnBeganEventArgs args)
{
    TurnBegan?.Invoke(this, args);
}

public EventHandle<TurnBeganEventArgs> SubscribeToTurnBegan(EventHandleDelegate<TurnBeganEventArgs> action)
{
    var handler = new EventHandle<TurnBeganEventArgs>(action, () => TurnBegan -= action);
    TurnBegan += handler.Invoke;
    return handler;
} 
    #endregion Code for event TurnBegan
}
/// <summary>
/// (object sender, CardPlayedEventArgs) args)
/// </summary>
public class OnCardPlayedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((CardEventsBase)events).SubscribeToCardPlayed(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(CardPlayedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, CardPlayedEventArgs) args)");
        }
        return ((CardEventsBase)events).SubscribeToCardPlayed(delegate(object sender, CardPlayedEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void CardPlayedEvent (object sender, CardPlayedEventArgs args);

    public class CardPlayedEventArgs {        public  IEntity CardId { get; }
        public  IEntity Target { get; }
        public  CardPlayedEventArgs (IEntity CardId, IEntity Target   ){
                  this.CardId = CardId; 
              this.Target = Target; 
}

        }/// <summary>
/// (object sender, CardDiscardedEventArgs) args)
/// </summary>
public class OnCardDiscardedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((CardEventsBase)events).SubscribeToCardDiscarded(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(CardDiscardedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, CardDiscardedEventArgs) args)");
        }
        return ((CardEventsBase)events).SubscribeToCardDiscarded(delegate(object sender, CardDiscardedEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void CardDiscardedEvent (object sender, CardDiscardedEventArgs args);

    public class CardDiscardedEventArgs {        public  IEntity CardId { get; }
        public  CardDiscardedEventArgs (IEntity CardId   ){
                  this.CardId = CardId; 
}

        }/// <summary>
/// (object sender, RequestDealDamageEventArgs) args)
/// </summary>
public class OnRequestDealDamageAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((CardEventsBase)events).SubscribeToRequestDealDamage(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(RequestDealDamageEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, RequestDealDamageEventArgs) args)");
        }
        return ((CardEventsBase)events).SubscribeToRequestDealDamage(delegate(object sender, RequestDealDamageEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void RequestDealDamageEvent (object sender, RequestDealDamageEventArgs args);

    public class RequestDealDamageEventArgs {        public  int Amount { get; }
        public  IEntity Source { get; }
        public  IEntity Target { get; }
        public  List<float> Multiplier { get; set;} 
=new List<float>();        public  List<int> Clamps { get; set;} 
=new List<int>();        public  RequestDealDamageEventArgs (int Amount, IEntity Source, IEntity Target   ){
                  this.Amount = Amount; 
              this.Source = Source; 
              this.Target = Target; 
}

        }/// <summary>
/// (object sender, EntityKilledEventArgs) args)
/// </summary>
public class OnEntityKilledAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((CardEventsBase)events).SubscribeToEntityKilled(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(EntityKilledEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, EntityKilledEventArgs) args)");
        }
        return ((CardEventsBase)events).SubscribeToEntityKilled(delegate(object sender, EntityKilledEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void EntityKilledEvent (object sender, EntityKilledEventArgs args);

    public class EntityKilledEventArgs {        public  IEntity Entity { get; }
        public  IEntity Source { get; }
        public  EntityKilledEventArgs (IEntity Entity, IEntity Source   ){
                  this.Entity = Entity; 
              this.Source = Source; 
}

        }/// <summary>
/// (object sender, DamageDealtEventArgs) args)
/// </summary>
public class OnDamageDealtAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((CardEventsBase)events).SubscribeToDamageDealt(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(DamageDealtEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, DamageDealtEventArgs) args)");
        }
        return ((CardEventsBase)events).SubscribeToDamageDealt(delegate(object sender, DamageDealtEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void DamageDealtEvent (object sender, DamageDealtEventArgs args);

    public class DamageDealtEventArgs {        public  IEntity EntityId { get; }
        public  IEntity SourceEntityId { get; }
        public  int Amount { get; }
        public  DamageDealtEventArgs (IEntity EntityId, IEntity SourceEntityId, int Amount   ){
                  this.EntityId = EntityId; 
              this.SourceEntityId = SourceEntityId; 
              this.Amount = Amount; 
}

        }/// <summary>
/// (object sender, TurnEndedEventArgs) args)
/// </summary>
public class OnTurnEndedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((CardEventsBase)events).SubscribeToTurnEnded(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(TurnEndedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, TurnEndedEventArgs) args)");
        }
        return ((CardEventsBase)events).SubscribeToTurnEnded(delegate(object sender, TurnEndedEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void TurnEndedEvent (object sender, TurnEndedEventArgs args);

    public class TurnEndedEventArgs {        }/// <summary>
/// (object sender, TurnBeganEventArgs) args)
/// </summary>
public class OnTurnBeganAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((CardEventsBase)events).SubscribeToTurnBegan(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(TurnBeganEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, TurnBeganEventArgs) args)");
        }
        return ((CardEventsBase)events).SubscribeToTurnBegan(delegate(object sender, TurnBeganEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void TurnBeganEvent (object sender, TurnBeganEventArgs args);

    public class TurnBeganEventArgs {        }


//TODO: generate attributes with static override functions that get the event handle from a game context.
//TODO: add generation of invoke functions.? 

 }

    

 