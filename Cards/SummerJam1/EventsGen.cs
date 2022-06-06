
// ReSharper disable RedundantUsingDirective
// ReSharper disable PossibleNullReferenceException
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantCast
using System;
using System.Collections.Generic;
using System.Reflection;
using Api;


namespace SummerJam1{
public abstract class SummerJam1EventsBase : CardsAndPiles.CardEvents{
    #region Code for event UnitCreated
private event UnitCreatedEvent UnitCreated;
internal virtual void OnUnitCreated(UnitCreatedEventArgs args)
{
    UnitCreated?.Invoke(this, args);
}

public EventHandle SubscribeToUnitCreated(UnitCreatedEvent action)
{
    UnitCreated += action;
    return new EventHandle(() => UnitCreated -= action);
} 
    #endregion Code for event UnitCreated
    #region Code for event TurnEnded
private event TurnEndedEvent TurnEnded;
internal virtual void OnTurnEnded(TurnEndedEventArgs args)
{
    TurnEnded?.Invoke(this, args);
}

public EventHandle SubscribeToTurnEnded(TurnEndedEvent action)
{
    TurnEnded += action;
    return new EventHandle(() => TurnEnded -= action);
} 
    #endregion Code for event TurnEnded
    #region Code for event TurnBegan
private event TurnBeganEvent TurnBegan;
internal virtual void OnTurnBegan(TurnBeganEventArgs args)
{
    TurnBegan?.Invoke(this, args);
}

public EventHandle SubscribeToTurnBegan(TurnBeganEvent action)
{
    TurnBegan += action;
    return new EventHandle(() => TurnBegan -= action);
} 
    #endregion Code for event TurnBegan
    #region Code for event IntentStarted
private event IntentStartedEvent IntentStarted;
internal virtual void OnIntentStarted(IntentStartedEventArgs args)
{
    IntentStarted?.Invoke(this, args);
}

public EventHandle SubscribeToIntentStarted(IntentStartedEvent action)
{
    IntentStarted += action;
    return new EventHandle(() => IntentStarted -= action);
} 
    #endregion Code for event IntentStarted
}
/// <summary>
/// (object sender, UnitCreatedEventArgs) args)
/// </summary>
public class OnUnitCreatedAttribute : EventsBaseAttribute {
    public override EventHandle GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToUnitCreated(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(UnitCreatedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, UnitCreatedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToUnitCreated(delegate(object sender, UnitCreatedEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    public delegate void UnitCreatedEvent (object sender, UnitCreatedEventArgs args);

    public class UnitCreatedEventArgs {        public  IEntity Entity { get; }
        public  UnitCreatedEventArgs (IEntity Entity   ){
                  this.Entity = Entity; 
}

        }/// <summary>
/// (object sender, TurnEndedEventArgs) args)
/// </summary>
public class OnTurnEndedAttribute : EventsBaseAttribute {
    public override EventHandle GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToTurnEnded(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(TurnEndedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, TurnEndedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToTurnEnded(delegate(object sender, TurnEndedEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    public delegate void TurnEndedEvent (object sender, TurnEndedEventArgs args);

    public class TurnEndedEventArgs {        }/// <summary>
/// (object sender, TurnBeganEventArgs) args)
/// </summary>
public class OnTurnBeganAttribute : EventsBaseAttribute {
    public override EventHandle GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToTurnBegan(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(TurnBeganEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, TurnBeganEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToTurnBegan(delegate(object sender, TurnBeganEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    public delegate void TurnBeganEvent (object sender, TurnBeganEventArgs args);

    public class TurnBeganEventArgs {        }/// <summary>
/// (object sender, IntentStartedEventArgs) args)
/// </summary>
public class OnIntentStartedAttribute : EventsBaseAttribute {
    public override EventHandle GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToIntentStarted(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(IntentStartedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, IntentStartedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToIntentStarted(delegate(object sender, IntentStartedEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    public delegate void IntentStartedEvent (object sender, IntentStartedEventArgs args);

    public class IntentStartedEventArgs {        public  IEntity Entity { get; }
        public  IntentStartedEventArgs (IEntity Entity   ){
                  this.Entity = Entity; 
}

        }


//TODO: generate attributes with static override functions that get the event handle from a game context.
//TODO: add generation of invoke functions.? 

 }

    

 