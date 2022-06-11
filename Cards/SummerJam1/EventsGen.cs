
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
private event EventHandleDelegate<UnitCreatedEventArgs> UnitCreated;
public virtual void OnUnitCreated(UnitCreatedEventArgs args)
{
    UnitCreated?.Invoke(this, args);
}

public EventHandle<UnitCreatedEventArgs> SubscribeToUnitCreated(EventHandleDelegate<UnitCreatedEventArgs> action)
{
    var handler = new EventHandle<UnitCreatedEventArgs>(action, () => UnitCreated -= action);
    UnitCreated += handler.Invoke;
    return handler;
} 
    #endregion Code for event UnitCreated
    #region Code for event IntentStarted
private event EventHandleDelegate<IntentStartedEventArgs> IntentStarted;
public virtual void OnIntentStarted(IntentStartedEventArgs args)
{
    IntentStarted?.Invoke(this, args);
}

public EventHandle<IntentStartedEventArgs> SubscribeToIntentStarted(EventHandleDelegate<IntentStartedEventArgs> action)
{
    var handler = new EventHandle<IntentStartedEventArgs>(action, () => IntentStarted -= action);
    IntentStarted += handler.Invoke;
    return handler;
} 
    #endregion Code for event IntentStarted
    #region Code for event UnitTransformed
private event EventHandleDelegate<UnitTransformedEventArgs> UnitTransformed;
public virtual void OnUnitTransformed(UnitTransformedEventArgs args)
{
    UnitTransformed?.Invoke(this, args);
}

public EventHandle<UnitTransformedEventArgs> SubscribeToUnitTransformed(EventHandleDelegate<UnitTransformedEventArgs> action)
{
    var handler = new EventHandle<UnitTransformedEventArgs>(action, () => UnitTransformed -= action);
    UnitTransformed += handler.Invoke;
    return handler;
} 
    #endregion Code for event UnitTransformed
    #region Code for event BattleEnded
private event EventHandleDelegate<BattleEndedEventArgs> BattleEnded;
public virtual void OnBattleEnded(BattleEndedEventArgs args)
{
    BattleEnded?.Invoke(this, args);
}

public EventHandle<BattleEndedEventArgs> SubscribeToBattleEnded(EventHandleDelegate<BattleEndedEventArgs> action)
{
    var handler = new EventHandle<BattleEndedEventArgs>(action, () => BattleEnded -= action);
    BattleEnded += handler.Invoke;
    return handler;
} 
    #endregion Code for event BattleEnded
    #region Code for event BattleStarted
private event EventHandleDelegate<BattleStartedEventArgs> BattleStarted;
public virtual void OnBattleStarted(BattleStartedEventArgs args)
{
    BattleStarted?.Invoke(this, args);
}

public EventHandle<BattleStartedEventArgs> SubscribeToBattleStarted(EventHandleDelegate<BattleStartedEventArgs> action)
{
    var handler = new EventHandle<BattleStartedEventArgs>(action, () => BattleStarted -= action);
    BattleStarted += handler.Invoke;
    return handler;
} 
    #endregion Code for event BattleStarted
}
/// <summary>
/// (object sender, UnitCreatedEventArgs) args)
/// </summary>
public class OnUnitCreatedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
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
    //public delegate void UnitCreatedEvent (object sender, UnitCreatedEventArgs args);

    public class UnitCreatedEventArgs {        public  IEntity Entity { get; }
        public  UnitCreatedEventArgs (IEntity Entity   ){
                  this.Entity = Entity; 
}

        }/// <summary>
/// (object sender, IntentStartedEventArgs) args)
/// </summary>
public class OnIntentStartedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
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
    //public delegate void IntentStartedEvent (object sender, IntentStartedEventArgs args);

    public class IntentStartedEventArgs {        public  IEntity Entity { get; }
        public  IntentStartedEventArgs (IEntity Entity   ){
                  this.Entity = Entity; 
}

        }/// <summary>
/// (object sender, UnitTransformedEventArgs) args)
/// </summary>
public class OnUnitTransformedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToUnitTransformed(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(UnitTransformedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, UnitTransformedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToUnitTransformed(delegate(object sender, UnitTransformedEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void UnitTransformedEvent (object sender, UnitTransformedEventArgs args);

    public class UnitTransformedEventArgs {        public  IEntity Entity { get; }
        public  UnitTransformedEventArgs (IEntity Entity   ){
                  this.Entity = Entity; 
}

        }/// <summary>
/// (object sender, BattleEndedEventArgs) args)
/// </summary>
public class OnBattleEndedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToBattleEnded(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(BattleEndedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, BattleEndedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToBattleEnded(delegate(object sender, BattleEndedEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void BattleEndedEvent (object sender, BattleEndedEventArgs args);

    public class BattleEndedEventArgs {        public  bool Victory { get; }
        public  BattleEndedEventArgs (bool Victory   ){
                  this.Victory = Victory; 
}

        }/// <summary>
/// (object sender, BattleStartedEventArgs) args)
/// </summary>
public class OnBattleStartedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToBattleStarted(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(BattleStartedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, BattleStartedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToBattleStarted(delegate(object sender, BattleStartedEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void BattleStartedEvent (object sender, BattleStartedEventArgs args);

    public class BattleStartedEventArgs {        public  BattleContainer Battle { get; }
        public  BattleStartedEventArgs (BattleContainer Battle   ){
                  this.Battle = Battle; 
}

        }


//TODO: generate attributes with static override functions that get the event handle from a game context.
//TODO: add generation of invoke functions.? 

 }

    

 