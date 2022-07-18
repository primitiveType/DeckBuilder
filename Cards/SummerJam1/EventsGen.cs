
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
    #region Code for event GameEnded
private event EventHandleDelegate<GameEndedEventArgs> GameEnded;
public virtual void OnGameEnded(GameEndedEventArgs args)
{
    GameEnded?.Invoke(this, args);
}

public EventHandle<GameEndedEventArgs> SubscribeToGameEnded(EventHandleDelegate<GameEndedEventArgs> action)
{
    var handler = new EventHandle<GameEndedEventArgs>(action, () => GameEnded -= action);
    GameEnded += handler.Invoke;
    return handler;
} 
    #endregion Code for event GameEnded
    #region Code for event GameStarted
private event EventHandleDelegate<GameStartedEventArgs> GameStarted;
public virtual void OnGameStarted(GameStartedEventArgs args)
{
    GameStarted?.Invoke(this, args);
}

public EventHandle<GameStartedEventArgs> SubscribeToGameStarted(EventHandleDelegate<GameStartedEventArgs> action)
{
    var handler = new EventHandle<GameStartedEventArgs>(action, () => GameStarted -= action);
    GameStarted += handler.Invoke;
    return handler;
} 
    #endregion Code for event GameStarted
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
    #region Code for event SomeTwitterEvent
private event EventHandleDelegate<SomeTwitterEventEventArgs> SomeTwitterEvent;
public virtual void OnSomeTwitterEvent(SomeTwitterEventEventArgs args)
{
    SomeTwitterEvent?.Invoke(this, args);
}

public EventHandle<SomeTwitterEventEventArgs> SubscribeToSomeTwitterEvent(EventHandleDelegate<SomeTwitterEventEventArgs> action)
{
    var handler = new EventHandle<SomeTwitterEventEventArgs>(action, () => SomeTwitterEvent -= action);
    SomeTwitterEvent += handler.Invoke;
    return handler;
} 
    #endregion Code for event SomeTwitterEvent
    #region Code for event LeaveBattle
private event EventHandleDelegate<LeaveBattleEventArgs> LeaveBattle;
public virtual void OnLeaveBattle(LeaveBattleEventArgs args)
{
    LeaveBattle?.Invoke(this, args);
}

public EventHandle<LeaveBattleEventArgs> SubscribeToLeaveBattle(EventHandleDelegate<LeaveBattleEventArgs> action)
{
    var handler = new EventHandle<LeaveBattleEventArgs>(action, () => LeaveBattle -= action);
    LeaveBattle += handler.Invoke;
    return handler;
} 
    #endregion Code for event LeaveBattle
    #region Code for event RequestRemoveCard
private event EventHandleDelegate<RequestRemoveCardEventArgs> RequestRemoveCard;
public virtual void OnRequestRemoveCard(RequestRemoveCardEventArgs args)
{
    RequestRemoveCard?.Invoke(this, args);
}

public EventHandle<RequestRemoveCardEventArgs> SubscribeToRequestRemoveCard(EventHandleDelegate<RequestRemoveCardEventArgs> action)
{
    var handler = new EventHandle<RequestRemoveCardEventArgs>(action, () => RequestRemoveCard -= action);
    RequestRemoveCard += handler.Invoke;
    return handler;
} 
    #endregion Code for event RequestRemoveCard
    #region Code for event AttackPhaseStarted
private event EventHandleDelegate<AttackPhaseStartedEventArgs> AttackPhaseStarted;
public virtual void OnAttackPhaseStarted(AttackPhaseStartedEventArgs args)
{
    AttackPhaseStarted?.Invoke(this, args);
}

public EventHandle<AttackPhaseStartedEventArgs> SubscribeToAttackPhaseStarted(EventHandleDelegate<AttackPhaseStartedEventArgs> action)
{
    var handler = new EventHandle<AttackPhaseStartedEventArgs>(action, () => AttackPhaseStarted -= action);
    AttackPhaseStarted += handler.Invoke;
    return handler;
} 
    #endregion Code for event AttackPhaseStarted
    #region Code for event AttackPhaseEnded
private event EventHandleDelegate<AttackPhaseEndedEventArgs> AttackPhaseEnded;
public virtual void OnAttackPhaseEnded(AttackPhaseEndedEventArgs args)
{
    AttackPhaseEnded?.Invoke(this, args);
}

public EventHandle<AttackPhaseEndedEventArgs> SubscribeToAttackPhaseEnded(EventHandleDelegate<AttackPhaseEndedEventArgs> action)
{
    var handler = new EventHandle<AttackPhaseEndedEventArgs>(action, () => AttackPhaseEnded -= action);
    AttackPhaseEnded += handler.Invoke;
    return handler;
} 
    #endregion Code for event AttackPhaseEnded
    #region Code for event RelicCreated
private event EventHandleDelegate<RelicCreatedEventArgs> RelicCreated;
public virtual void OnRelicCreated(RelicCreatedEventArgs args)
{
    RelicCreated?.Invoke(this, args);
}

public EventHandle<RelicCreatedEventArgs> SubscribeToRelicCreated(EventHandleDelegate<RelicCreatedEventArgs> action)
{
    var handler = new EventHandle<RelicCreatedEventArgs>(action, () => RelicCreated -= action);
    RelicCreated += handler.Invoke;
    return handler;
} 
    #endregion Code for event RelicCreated
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
/// (object sender, GameEndedEventArgs) args)
/// </summary>
public class OnGameEndedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToGameEnded(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(GameEndedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, GameEndedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToGameEnded(delegate(object sender, GameEndedEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void GameEndedEvent (object sender, GameEndedEventArgs args);

    public class GameEndedEventArgs {        public  bool Victory { get; }
        public  GameEndedEventArgs (bool Victory   ){
                  this.Victory = Victory; 
}

        }/// <summary>
/// (object sender, GameStartedEventArgs) args)
/// </summary>
public class OnGameStartedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToGameStarted(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(GameStartedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, GameStartedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToGameStarted(delegate(object sender, GameStartedEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void GameStartedEvent (object sender, GameStartedEventArgs args);

    public class GameStartedEventArgs {        }/// <summary>
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

    public class BattleStartedEventArgs {        }/// <summary>
/// (object sender, SomeTwitterEventEventArgs) args)
/// </summary>
public class OnSomeTwitterEventAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToSomeTwitterEvent(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(SomeTwitterEventEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, SomeTwitterEventEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToSomeTwitterEvent(delegate(object sender, SomeTwitterEventEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void SomeTwitterEventEvent (object sender, SomeTwitterEventEventArgs args);

    public class SomeTwitterEventEventArgs {        }/// <summary>
/// (object sender, LeaveBattleEventArgs) args)
/// </summary>
public class OnLeaveBattleAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToLeaveBattle(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(LeaveBattleEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, LeaveBattleEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToLeaveBattle(delegate(object sender, LeaveBattleEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void LeaveBattleEvent (object sender, LeaveBattleEventArgs args);

    public class LeaveBattleEventArgs {        }/// <summary>
/// (object sender, RequestRemoveCardEventArgs) args)
/// </summary>
public class OnRequestRemoveCardAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToRequestRemoveCard(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(RequestRemoveCardEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, RequestRemoveCardEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToRequestRemoveCard(delegate(object sender, RequestRemoveCardEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void RequestRemoveCardEvent (object sender, RequestRemoveCardEventArgs args);

    public class RequestRemoveCardEventArgs {        }/// <summary>
/// (object sender, AttackPhaseStartedEventArgs) args)
/// </summary>
public class OnAttackPhaseStartedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToAttackPhaseStarted(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(AttackPhaseStartedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, AttackPhaseStartedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToAttackPhaseStarted(delegate(object sender, AttackPhaseStartedEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void AttackPhaseStartedEvent (object sender, AttackPhaseStartedEventArgs args);

    public class AttackPhaseStartedEventArgs {        }/// <summary>
/// (object sender, AttackPhaseEndedEventArgs) args)
/// </summary>
public class OnAttackPhaseEndedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToAttackPhaseEnded(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(AttackPhaseEndedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, AttackPhaseEndedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToAttackPhaseEnded(delegate(object sender, AttackPhaseEndedEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void AttackPhaseEndedEvent (object sender, AttackPhaseEndedEventArgs args);

    public class AttackPhaseEndedEventArgs {        }/// <summary>
/// (object sender, RelicCreatedEventArgs) args)
/// </summary>
public class OnRelicCreatedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToRelicCreated(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(RelicCreatedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, RelicCreatedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToRelicCreated(delegate(object sender, RelicCreatedEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void RelicCreatedEvent (object sender, RelicCreatedEventArgs args);

    public class RelicCreatedEventArgs {        public  IEntity Relic { get; }
        public  RelicCreatedEventArgs (IEntity Relic   ){
                  this.Relic = Relic; 
}

        }


//TODO: generate attributes with static override functions that get the event handle from a game context.
//TODO: add generation of invoke functions.? 

 }

    

 
