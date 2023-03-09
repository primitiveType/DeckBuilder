

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
    #region Code for event RequestMoveUnit
private event EventHandleDelegate<RequestMoveUnitEventArgs> RequestMoveUnit;
public virtual void OnRequestMoveUnit(RequestMoveUnitEventArgs args)
{
    RequestMoveUnit?.Invoke(this, args);
}

public EventHandle<RequestMoveUnitEventArgs> SubscribeToRequestMoveUnit(EventHandleDelegate<RequestMoveUnitEventArgs> action)
{
    var handler = new EventHandle<RequestMoveUnitEventArgs>(action, () => RequestMoveUnit -= action);
    RequestMoveUnit += handler.Invoke;
    return handler;
} 
    #endregion Code for event RequestMoveUnit
    #region Code for event UnitMoved
private event EventHandleDelegate<UnitMovedEventArgs> UnitMoved;
public virtual void OnUnitMoved(UnitMovedEventArgs args)
{
    UnitMoved?.Invoke(this, args);
}

public EventHandle<UnitMovedEventArgs> SubscribeToUnitMoved(EventHandleDelegate<UnitMovedEventArgs> action)
{
    var handler = new EventHandle<UnitMovedEventArgs>(action, () => UnitMoved -= action);
    UnitMoved += handler.Invoke;
    return handler;
} 
    #endregion Code for event UnitMoved
    #region Code for event BeatOverloaded
private event EventHandleDelegate<BeatOverloadedEventArgs> BeatOverloaded;
public virtual void OnBeatOverloaded(BeatOverloadedEventArgs args)
{
    BeatOverloaded?.Invoke(this, args);
}

public EventHandle<BeatOverloadedEventArgs> SubscribeToBeatOverloaded(EventHandleDelegate<BeatOverloadedEventArgs> action)
{
    var handler = new EventHandle<BeatOverloadedEventArgs>(action, () => BeatOverloaded -= action);
    BeatOverloaded += handler.Invoke;
    return handler;
} 
    #endregion Code for event BeatOverloaded
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
    #region Code for event DungeonPhaseStarted
private event EventHandleDelegate<DungeonPhaseStartedEventArgs> DungeonPhaseStarted;
public virtual void OnDungeonPhaseStarted(DungeonPhaseStartedEventArgs args)
{
    DungeonPhaseStarted?.Invoke(this, args);
}

public EventHandle<DungeonPhaseStartedEventArgs> SubscribeToDungeonPhaseStarted(EventHandleDelegate<DungeonPhaseStartedEventArgs> action)
{
    var handler = new EventHandle<DungeonPhaseStartedEventArgs>(action, () => DungeonPhaseStarted -= action);
    DungeonPhaseStarted += handler.Invoke;
    return handler;
} 
    #endregion Code for event DungeonPhaseStarted
    #region Code for event DungeonPhaseEnded
private event EventHandleDelegate<DungeonPhaseEndedEventArgs> DungeonPhaseEnded;
public virtual void OnDungeonPhaseEnded(DungeonPhaseEndedEventArgs args)
{
    DungeonPhaseEnded?.Invoke(this, args);
}

public EventHandle<DungeonPhaseEndedEventArgs> SubscribeToDungeonPhaseEnded(EventHandleDelegate<DungeonPhaseEndedEventArgs> action)
{
    var handler = new EventHandle<DungeonPhaseEndedEventArgs>(action, () => DungeonPhaseEnded -= action);
    DungeonPhaseEnded += handler.Invoke;
    return handler;
} 
    #endregion Code for event DungeonPhaseEnded
    #region Code for event MovementPhaseBegan
private event EventHandleDelegate<MovementPhaseBeganEventArgs> MovementPhaseBegan;
public virtual void OnMovementPhaseBegan(MovementPhaseBeganEventArgs args)
{
    MovementPhaseBegan?.Invoke(this, args);
}

public EventHandle<MovementPhaseBeganEventArgs> SubscribeToMovementPhaseBegan(EventHandleDelegate<MovementPhaseBeganEventArgs> action)
{
    var handler = new EventHandle<MovementPhaseBeganEventArgs>(action, () => MovementPhaseBegan -= action);
    MovementPhaseBegan += handler.Invoke;
    return handler;
} 
    #endregion Code for event MovementPhaseBegan
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
    public override IDisposable GetEventHandle(MethodInfo attached, IEventfulComponent instance, EventsBase events)
    {
        instance.EventEntrance.Add(Id, 0);
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToUnitCreated(delegate
            {
                if(!instance.Enabled){
                    return;
                }
                if(instance.EventEntrance[Id] > 0){
                    Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                    return;
                }
                instance.EventEntrance[Id]++;
                attached.Invoke(instance, Array.Empty<object>());
                instance.EventEntrance[Id]--;
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(UnitCreatedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, UnitCreatedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToUnitCreated(delegate(object sender, UnitCreatedEventArgs args)
        {
            if(!instance.Enabled){
                return;
            }
            if(instance.EventEntrance[Id] > 0){
                Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                return;
            }
            instance.EventEntrance[Id]++;
            attached.Invoke(instance, new[] { sender, args });
            instance.EventEntrance[Id]--;
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
    public override IDisposable GetEventHandle(MethodInfo attached, IEventfulComponent instance, EventsBase events)
    {
        instance.EventEntrance.Add(Id, 0);
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToIntentStarted(delegate
            {
                if(!instance.Enabled){
                    return;
                }
                if(instance.EventEntrance[Id] > 0){
                    Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                    return;
                }
                instance.EventEntrance[Id]++;
                attached.Invoke(instance, Array.Empty<object>());
                instance.EventEntrance[Id]--;
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(IntentStartedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, IntentStartedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToIntentStarted(delegate(object sender, IntentStartedEventArgs args)
        {
            if(!instance.Enabled){
                return;
            }
            if(instance.EventEntrance[Id] > 0){
                Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                return;
            }
            instance.EventEntrance[Id]++;
            attached.Invoke(instance, new[] { sender, args });
            instance.EventEntrance[Id]--;
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
    public override IDisposable GetEventHandle(MethodInfo attached, IEventfulComponent instance, EventsBase events)
    {
        instance.EventEntrance.Add(Id, 0);
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToUnitTransformed(delegate
            {
                if(!instance.Enabled){
                    return;
                }
                if(instance.EventEntrance[Id] > 0){
                    Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                    return;
                }
                instance.EventEntrance[Id]++;
                attached.Invoke(instance, Array.Empty<object>());
                instance.EventEntrance[Id]--;
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(UnitTransformedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, UnitTransformedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToUnitTransformed(delegate(object sender, UnitTransformedEventArgs args)
        {
            if(!instance.Enabled){
                return;
            }
            if(instance.EventEntrance[Id] > 0){
                Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                return;
            }
            instance.EventEntrance[Id]++;
            attached.Invoke(instance, new[] { sender, args });
            instance.EventEntrance[Id]--;
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
    public override IDisposable GetEventHandle(MethodInfo attached, IEventfulComponent instance, EventsBase events)
    {
        instance.EventEntrance.Add(Id, 0);
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToBattleEnded(delegate
            {
                if(!instance.Enabled){
                    return;
                }
                if(instance.EventEntrance[Id] > 0){
                    Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                    return;
                }
                instance.EventEntrance[Id]++;
                attached.Invoke(instance, Array.Empty<object>());
                instance.EventEntrance[Id]--;
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(BattleEndedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, BattleEndedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToBattleEnded(delegate(object sender, BattleEndedEventArgs args)
        {
            if(!instance.Enabled){
                return;
            }
            if(instance.EventEntrance[Id] > 0){
                Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                return;
            }
            instance.EventEntrance[Id]++;
            attached.Invoke(instance, new[] { sender, args });
            instance.EventEntrance[Id]--;
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
    public override IDisposable GetEventHandle(MethodInfo attached, IEventfulComponent instance, EventsBase events)
    {
        instance.EventEntrance.Add(Id, 0);
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToGameEnded(delegate
            {
                if(!instance.Enabled){
                    return;
                }
                if(instance.EventEntrance[Id] > 0){
                    Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                    return;
                }
                instance.EventEntrance[Id]++;
                attached.Invoke(instance, Array.Empty<object>());
                instance.EventEntrance[Id]--;
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(GameEndedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, GameEndedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToGameEnded(delegate(object sender, GameEndedEventArgs args)
        {
            if(!instance.Enabled){
                return;
            }
            if(instance.EventEntrance[Id] > 0){
                Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                return;
            }
            instance.EventEntrance[Id]++;
            attached.Invoke(instance, new[] { sender, args });
            instance.EventEntrance[Id]--;
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
    public override IDisposable GetEventHandle(MethodInfo attached, IEventfulComponent instance, EventsBase events)
    {
        instance.EventEntrance.Add(Id, 0);
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToGameStarted(delegate
            {
                if(!instance.Enabled){
                    return;
                }
                if(instance.EventEntrance[Id] > 0){
                    Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                    return;
                }
                instance.EventEntrance[Id]++;
                attached.Invoke(instance, Array.Empty<object>());
                instance.EventEntrance[Id]--;
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(GameStartedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, GameStartedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToGameStarted(delegate(object sender, GameStartedEventArgs args)
        {
            if(!instance.Enabled){
                return;
            }
            if(instance.EventEntrance[Id] > 0){
                Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                return;
            }
            instance.EventEntrance[Id]++;
            attached.Invoke(instance, new[] { sender, args });
            instance.EventEntrance[Id]--;
        });
    }


}
    //public delegate void GameStartedEvent (object sender, GameStartedEventArgs args);

    public class GameStartedEventArgs {        }/// <summary>
/// (object sender, BattleStartedEventArgs) args)
/// </summary>
public class OnBattleStartedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, IEventfulComponent instance, EventsBase events)
    {
        instance.EventEntrance.Add(Id, 0);
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToBattleStarted(delegate
            {
                if(!instance.Enabled){
                    return;
                }
                if(instance.EventEntrance[Id] > 0){
                    Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                    return;
                }
                instance.EventEntrance[Id]++;
                attached.Invoke(instance, Array.Empty<object>());
                instance.EventEntrance[Id]--;
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(BattleStartedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, BattleStartedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToBattleStarted(delegate(object sender, BattleStartedEventArgs args)
        {
            if(!instance.Enabled){
                return;
            }
            if(instance.EventEntrance[Id] > 0){
                Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                return;
            }
            instance.EventEntrance[Id]++;
            attached.Invoke(instance, new[] { sender, args });
            instance.EventEntrance[Id]--;
        });
    }


}
    //public delegate void BattleStartedEvent (object sender, BattleStartedEventArgs args);

    public class BattleStartedEventArgs {        }/// <summary>
/// (object sender, SomeTwitterEventEventArgs) args)
/// </summary>
public class OnSomeTwitterEventAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, IEventfulComponent instance, EventsBase events)
    {
        instance.EventEntrance.Add(Id, 0);
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToSomeTwitterEvent(delegate
            {
                if(!instance.Enabled){
                    return;
                }
                if(instance.EventEntrance[Id] > 0){
                    Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                    return;
                }
                instance.EventEntrance[Id]++;
                attached.Invoke(instance, Array.Empty<object>());
                instance.EventEntrance[Id]--;
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(SomeTwitterEventEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, SomeTwitterEventEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToSomeTwitterEvent(delegate(object sender, SomeTwitterEventEventArgs args)
        {
            if(!instance.Enabled){
                return;
            }
            if(instance.EventEntrance[Id] > 0){
                Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                return;
            }
            instance.EventEntrance[Id]++;
            attached.Invoke(instance, new[] { sender, args });
            instance.EventEntrance[Id]--;
        });
    }


}
    //public delegate void SomeTwitterEventEvent (object sender, SomeTwitterEventEventArgs args);

    public class SomeTwitterEventEventArgs {        }/// <summary>
/// (object sender, LeaveBattleEventArgs) args)
/// </summary>
public class OnLeaveBattleAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, IEventfulComponent instance, EventsBase events)
    {
        instance.EventEntrance.Add(Id, 0);
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToLeaveBattle(delegate
            {
                if(!instance.Enabled){
                    return;
                }
                if(instance.EventEntrance[Id] > 0){
                    Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                    return;
                }
                instance.EventEntrance[Id]++;
                attached.Invoke(instance, Array.Empty<object>());
                instance.EventEntrance[Id]--;
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(LeaveBattleEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, LeaveBattleEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToLeaveBattle(delegate(object sender, LeaveBattleEventArgs args)
        {
            if(!instance.Enabled){
                return;
            }
            if(instance.EventEntrance[Id] > 0){
                Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                return;
            }
            instance.EventEntrance[Id]++;
            attached.Invoke(instance, new[] { sender, args });
            instance.EventEntrance[Id]--;
        });
    }


}
    //public delegate void LeaveBattleEvent (object sender, LeaveBattleEventArgs args);

    public class LeaveBattleEventArgs {        }/// <summary>
/// (object sender, RequestMoveUnitEventArgs) args)
/// </summary>
public class OnRequestMoveUnitAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, IEventfulComponent instance, EventsBase events)
    {
        instance.EventEntrance.Add(Id, 0);
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToRequestMoveUnit(delegate
            {
                if(!instance.Enabled){
                    return;
                }
                if(instance.EventEntrance[Id] > 0){
                    Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                    return;
                }
                instance.EventEntrance[Id]++;
                attached.Invoke(instance, Array.Empty<object>());
                instance.EventEntrance[Id]--;
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(RequestMoveUnitEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, RequestMoveUnitEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToRequestMoveUnit(delegate(object sender, RequestMoveUnitEventArgs args)
        {
            if(!instance.Enabled){
                return;
            }
            if(instance.EventEntrance[Id] > 0){
                Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                return;
            }
            instance.EventEntrance[Id]++;
            attached.Invoke(instance, new[] { sender, args });
            instance.EventEntrance[Id]--;
        });
    }


}
    //public delegate void RequestMoveUnitEvent (object sender, RequestMoveUnitEventArgs args);

    public class RequestMoveUnitEventArgs {        public  IEntity CardId { get; }
        public  bool UsesMovement { get; }
        public  IEntity Target { get; }
        public  List<string> Blockers { get; set;} 
=new List<string>();        public  RequestMoveUnitEventArgs (IEntity CardId, bool UsesMovement, IEntity Target   ){
                  this.CardId = CardId; 
              this.UsesMovement = UsesMovement; 
              this.Target = Target; 
}

        }/// <summary>
/// (object sender, UnitMovedEventArgs) args)
/// </summary>
public class OnUnitMovedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, IEventfulComponent instance, EventsBase events)
    {
        instance.EventEntrance.Add(Id, 0);
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToUnitMoved(delegate
            {
                if(!instance.Enabled){
                    return;
                }
                if(instance.EventEntrance[Id] > 0){
                    Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                    return;
                }
                instance.EventEntrance[Id]++;
                attached.Invoke(instance, Array.Empty<object>());
                instance.EventEntrance[Id]--;
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(UnitMovedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, UnitMovedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToUnitMoved(delegate(object sender, UnitMovedEventArgs args)
        {
            if(!instance.Enabled){
                return;
            }
            if(instance.EventEntrance[Id] > 0){
                Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                return;
            }
            instance.EventEntrance[Id]++;
            attached.Invoke(instance, new[] { sender, args });
            instance.EventEntrance[Id]--;
        });
    }


}
    //public delegate void UnitMovedEvent (object sender, UnitMovedEventArgs args);

    public class UnitMovedEventArgs {        public  IEntity CardId { get; }
        public  bool UsesMovement { get; }
        public  IEntity Target { get; }
        public  UnitMovedEventArgs (IEntity CardId, bool UsesMovement, IEntity Target   ){
                  this.CardId = CardId; 
              this.UsesMovement = UsesMovement; 
              this.Target = Target; 
}

        }/// <summary>
/// (object sender, BeatOverloadedEventArgs) args)
/// </summary>
public class OnBeatOverloadedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, IEventfulComponent instance, EventsBase events)
    {
        instance.EventEntrance.Add(Id, 0);
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToBeatOverloaded(delegate
            {
                if(!instance.Enabled){
                    return;
                }
                if(instance.EventEntrance[Id] > 0){
                    Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                    return;
                }
                instance.EventEntrance[Id]++;
                attached.Invoke(instance, Array.Empty<object>());
                instance.EventEntrance[Id]--;
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(BeatOverloadedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, BeatOverloadedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToBeatOverloaded(delegate(object sender, BeatOverloadedEventArgs args)
        {
            if(!instance.Enabled){
                return;
            }
            if(instance.EventEntrance[Id] > 0){
                Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                return;
            }
            instance.EventEntrance[Id]++;
            attached.Invoke(instance, new[] { sender, args });
            instance.EventEntrance[Id]--;
        });
    }


}
    //public delegate void BeatOverloadedEvent (object sender, BeatOverloadedEventArgs args);

    public class BeatOverloadedEventArgs {        public  int Amount { get; }
        public  BeatOverloadedEventArgs (int Amount   ){
                  this.Amount = Amount; 
}

        }/// <summary>
/// (object sender, RequestRemoveCardEventArgs) args)
/// </summary>
public class OnRequestRemoveCardAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, IEventfulComponent instance, EventsBase events)
    {
        instance.EventEntrance.Add(Id, 0);
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToRequestRemoveCard(delegate
            {
                if(!instance.Enabled){
                    return;
                }
                if(instance.EventEntrance[Id] > 0){
                    Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                    return;
                }
                instance.EventEntrance[Id]++;
                attached.Invoke(instance, Array.Empty<object>());
                instance.EventEntrance[Id]--;
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(RequestRemoveCardEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, RequestRemoveCardEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToRequestRemoveCard(delegate(object sender, RequestRemoveCardEventArgs args)
        {
            if(!instance.Enabled){
                return;
            }
            if(instance.EventEntrance[Id] > 0){
                Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                return;
            }
            instance.EventEntrance[Id]++;
            attached.Invoke(instance, new[] { sender, args });
            instance.EventEntrance[Id]--;
        });
    }


}
    //public delegate void RequestRemoveCardEvent (object sender, RequestRemoveCardEventArgs args);

    public class RequestRemoveCardEventArgs {        }/// <summary>
/// (object sender, AttackPhaseStartedEventArgs) args)
/// </summary>
public class OnAttackPhaseStartedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, IEventfulComponent instance, EventsBase events)
    {
        instance.EventEntrance.Add(Id, 0);
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToAttackPhaseStarted(delegate
            {
                if(!instance.Enabled){
                    return;
                }
                if(instance.EventEntrance[Id] > 0){
                    Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                    return;
                }
                instance.EventEntrance[Id]++;
                attached.Invoke(instance, Array.Empty<object>());
                instance.EventEntrance[Id]--;
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(AttackPhaseStartedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, AttackPhaseStartedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToAttackPhaseStarted(delegate(object sender, AttackPhaseStartedEventArgs args)
        {
            if(!instance.Enabled){
                return;
            }
            if(instance.EventEntrance[Id] > 0){
                Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                return;
            }
            instance.EventEntrance[Id]++;
            attached.Invoke(instance, new[] { sender, args });
            instance.EventEntrance[Id]--;
        });
    }


}
    //public delegate void AttackPhaseStartedEvent (object sender, AttackPhaseStartedEventArgs args);

    public class AttackPhaseStartedEventArgs {        }/// <summary>
/// (object sender, AttackPhaseEndedEventArgs) args)
/// </summary>
public class OnAttackPhaseEndedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, IEventfulComponent instance, EventsBase events)
    {
        instance.EventEntrance.Add(Id, 0);
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToAttackPhaseEnded(delegate
            {
                if(!instance.Enabled){
                    return;
                }
                if(instance.EventEntrance[Id] > 0){
                    Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                    return;
                }
                instance.EventEntrance[Id]++;
                attached.Invoke(instance, Array.Empty<object>());
                instance.EventEntrance[Id]--;
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(AttackPhaseEndedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, AttackPhaseEndedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToAttackPhaseEnded(delegate(object sender, AttackPhaseEndedEventArgs args)
        {
            if(!instance.Enabled){
                return;
            }
            if(instance.EventEntrance[Id] > 0){
                Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                return;
            }
            instance.EventEntrance[Id]++;
            attached.Invoke(instance, new[] { sender, args });
            instance.EventEntrance[Id]--;
        });
    }


}
    //public delegate void AttackPhaseEndedEvent (object sender, AttackPhaseEndedEventArgs args);

    public class AttackPhaseEndedEventArgs {        }/// <summary>
/// (object sender, DungeonPhaseStartedEventArgs) args)
/// </summary>
public class OnDungeonPhaseStartedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, IEventfulComponent instance, EventsBase events)
    {
        instance.EventEntrance.Add(Id, 0);
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToDungeonPhaseStarted(delegate
            {
                if(!instance.Enabled){
                    return;
                }
                if(instance.EventEntrance[Id] > 0){
                    Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                    return;
                }
                instance.EventEntrance[Id]++;
                attached.Invoke(instance, Array.Empty<object>());
                instance.EventEntrance[Id]--;
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(DungeonPhaseStartedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, DungeonPhaseStartedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToDungeonPhaseStarted(delegate(object sender, DungeonPhaseStartedEventArgs args)
        {
            if(!instance.Enabled){
                return;
            }
            if(instance.EventEntrance[Id] > 0){
                Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                return;
            }
            instance.EventEntrance[Id]++;
            attached.Invoke(instance, new[] { sender, args });
            instance.EventEntrance[Id]--;
        });
    }


}
    //public delegate void DungeonPhaseStartedEvent (object sender, DungeonPhaseStartedEventArgs args);

    public class DungeonPhaseStartedEventArgs {        }/// <summary>
/// (object sender, DungeonPhaseEndedEventArgs) args)
/// </summary>
public class OnDungeonPhaseEndedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, IEventfulComponent instance, EventsBase events)
    {
        instance.EventEntrance.Add(Id, 0);
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToDungeonPhaseEnded(delegate
            {
                if(!instance.Enabled){
                    return;
                }
                if(instance.EventEntrance[Id] > 0){
                    Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                    return;
                }
                instance.EventEntrance[Id]++;
                attached.Invoke(instance, Array.Empty<object>());
                instance.EventEntrance[Id]--;
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(DungeonPhaseEndedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, DungeonPhaseEndedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToDungeonPhaseEnded(delegate(object sender, DungeonPhaseEndedEventArgs args)
        {
            if(!instance.Enabled){
                return;
            }
            if(instance.EventEntrance[Id] > 0){
                Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                return;
            }
            instance.EventEntrance[Id]++;
            attached.Invoke(instance, new[] { sender, args });
            instance.EventEntrance[Id]--;
        });
    }


}
    //public delegate void DungeonPhaseEndedEvent (object sender, DungeonPhaseEndedEventArgs args);

    public class DungeonPhaseEndedEventArgs {        }/// <summary>
/// (object sender, MovementPhaseBeganEventArgs) args)
/// </summary>
public class OnMovementPhaseBeganAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, IEventfulComponent instance, EventsBase events)
    {
        instance.EventEntrance.Add(Id, 0);
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToMovementPhaseBegan(delegate
            {
                if(!instance.Enabled){
                    return;
                }
                if(instance.EventEntrance[Id] > 0){
                    Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                    return;
                }
                instance.EventEntrance[Id]++;
                attached.Invoke(instance, Array.Empty<object>());
                instance.EventEntrance[Id]--;
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(MovementPhaseBeganEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, MovementPhaseBeganEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToMovementPhaseBegan(delegate(object sender, MovementPhaseBeganEventArgs args)
        {
            if(!instance.Enabled){
                return;
            }
            if(instance.EventEntrance[Id] > 0){
                Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                return;
            }
            instance.EventEntrance[Id]++;
            attached.Invoke(instance, new[] { sender, args });
            instance.EventEntrance[Id]--;
        });
    }


}
    //public delegate void MovementPhaseBeganEvent (object sender, MovementPhaseBeganEventArgs args);

    public class MovementPhaseBeganEventArgs {        }/// <summary>
/// (object sender, RelicCreatedEventArgs) args)
/// </summary>
public class OnRelicCreatedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, IEventfulComponent instance, EventsBase events)
    {
        instance.EventEntrance.Add(Id, 0);
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((SummerJam1EventsBase)events).SubscribeToRelicCreated(delegate
            {
                if(!instance.Enabled){
                    return;
                }
                if(instance.EventEntrance[Id] > 0){
                    Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                    return;
                }
                instance.EventEntrance[Id]++;
                attached.Invoke(instance, Array.Empty<object>());
                instance.EventEntrance[Id]--;
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(RelicCreatedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, RelicCreatedEventArgs) args)");
        }
        return ((SummerJam1EventsBase)events).SubscribeToRelicCreated(delegate(object sender, RelicCreatedEventArgs args)
        {
            if(!instance.Enabled){
                return;
            }
            if(instance.EventEntrance[Id] > 0){
                Logging.Log($"Preventing re-entrancy on event {Id} for component {instance.GetType()}.");
                return;
            }
            instance.EventEntrance[Id]++;
            attached.Invoke(instance, new[] { sender, args });
            instance.EventEntrance[Id]--;
        });
    }


}
    //public delegate void RelicCreatedEvent (object sender, RelicCreatedEventArgs args);

    public class RelicCreatedEventArgs {        public  IEntity Relic { get; }
        public  RelicCreatedEventArgs (IEntity Relic   ){
                  this.Relic = Relic; 
}

        }




 }

    

 
