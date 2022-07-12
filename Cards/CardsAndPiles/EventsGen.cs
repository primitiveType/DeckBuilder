
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
    #region Code for event RequestPlayCard
private event EventHandleDelegate<RequestPlayCardEventArgs> RequestPlayCard;
public virtual void OnRequestPlayCard(RequestPlayCardEventArgs args)
{
    RequestPlayCard?.Invoke(this, args);
}

public EventHandle<RequestPlayCardEventArgs> SubscribeToRequestPlayCard(EventHandleDelegate<RequestPlayCardEventArgs> action)
{
    var handler = new EventHandle<RequestPlayCardEventArgs>(action, () => RequestPlayCard -= action);
    RequestPlayCard += handler.Invoke;
    return handler;
} 
    #endregion Code for event RequestPlayCard
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
    #region Code for event CardCreated
private event EventHandleDelegate<CardCreatedEventArgs> CardCreated;
public virtual void OnCardCreated(CardCreatedEventArgs args)
{
    CardCreated?.Invoke(this, args);
}

public EventHandle<CardCreatedEventArgs> SubscribeToCardCreated(EventHandleDelegate<CardCreatedEventArgs> action)
{
    var handler = new EventHandle<CardCreatedEventArgs>(action, () => CardCreated -= action);
    CardCreated += handler.Invoke;
    return handler;
} 
    #endregion Code for event CardCreated
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
    #region Code for event CardExhausted
private event EventHandleDelegate<CardExhaustedEventArgs> CardExhausted;
public virtual void OnCardExhausted(CardExhaustedEventArgs args)
{
    CardExhausted?.Invoke(this, args);
}

public EventHandle<CardExhaustedEventArgs> SubscribeToCardExhausted(EventHandleDelegate<CardExhaustedEventArgs> action)
{
    var handler = new EventHandle<CardExhaustedEventArgs>(action, () => CardExhausted -= action);
    CardExhausted += handler.Invoke;
    return handler;
} 
    #endregion Code for event CardExhausted
    #region Code for event RequestDamageMultipliers
private event EventHandleDelegate<RequestDamageMultipliersEventArgs> RequestDamageMultipliers;
public virtual void OnRequestDamageMultipliers(RequestDamageMultipliersEventArgs args)
{
    RequestDamageMultipliers?.Invoke(this, args);
}

public EventHandle<RequestDamageMultipliersEventArgs> SubscribeToRequestDamageMultipliers(EventHandleDelegate<RequestDamageMultipliersEventArgs> action)
{
    var handler = new EventHandle<RequestDamageMultipliersEventArgs>(action, () => RequestDamageMultipliers -= action);
    RequestDamageMultipliers += handler.Invoke;
    return handler;
} 
    #endregion Code for event RequestDamageMultipliers
    #region Code for event RequestDamageReduction
private event EventHandleDelegate<RequestDamageReductionEventArgs> RequestDamageReduction;
public virtual void OnRequestDamageReduction(RequestDamageReductionEventArgs args)
{
    RequestDamageReduction?.Invoke(this, args);
}

public EventHandle<RequestDamageReductionEventArgs> SubscribeToRequestDamageReduction(EventHandleDelegate<RequestDamageReductionEventArgs> action)
{
    var handler = new EventHandle<RequestDamageReductionEventArgs>(action, () => RequestDamageReduction -= action);
    RequestDamageReduction += handler.Invoke;
    return handler;
} 
    #endregion Code for event RequestDamageReduction
    #region Code for event CardPlayFailed
private event EventHandleDelegate<CardPlayFailedEventArgs> CardPlayFailed;
public virtual void OnCardPlayFailed(CardPlayFailedEventArgs args)
{
    CardPlayFailed?.Invoke(this, args);
}

public EventHandle<CardPlayFailedEventArgs> SubscribeToCardPlayFailed(EventHandleDelegate<CardPlayFailedEventArgs> action)
{
    var handler = new EventHandle<CardPlayFailedEventArgs>(action, () => CardPlayFailed -= action);
    CardPlayFailed += handler.Invoke;
    return handler;
} 
    #endregion Code for event CardPlayFailed
    #region Code for event RequestHeal
private event EventHandleDelegate<RequestHealEventArgs> RequestHeal;
public virtual void OnRequestHeal(RequestHealEventArgs args)
{
    RequestHeal?.Invoke(this, args);
}

public EventHandle<RequestHealEventArgs> SubscribeToRequestHeal(EventHandleDelegate<RequestHealEventArgs> action)
{
    var handler = new EventHandle<RequestHealEventArgs>(action, () => RequestHeal -= action);
    RequestHeal += handler.Invoke;
    return handler;
} 
    #endregion Code for event RequestHeal
    #region Code for event ChooseCardsToDiscard
private event EventHandleDelegate<ChooseCardsToDiscardEventArgs> ChooseCardsToDiscard;
public virtual void OnChooseCardsToDiscard(ChooseCardsToDiscardEventArgs args)
{
    ChooseCardsToDiscard?.Invoke(this, args);
}

public EventHandle<ChooseCardsToDiscardEventArgs> SubscribeToChooseCardsToDiscard(EventHandleDelegate<ChooseCardsToDiscardEventArgs> action)
{
    var handler = new EventHandle<ChooseCardsToDiscardEventArgs>(action, () => ChooseCardsToDiscard -= action);
    ChooseCardsToDiscard += handler.Invoke;
    return handler;
} 
    #endregion Code for event ChooseCardsToDiscard
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
    #region Code for event HealDealt
private event EventHandleDelegate<HealDealtEventArgs> HealDealt;
public virtual void OnHealDealt(HealDealtEventArgs args)
{
    HealDealt?.Invoke(this, args);
}

public EventHandle<HealDealtEventArgs> SubscribeToHealDealt(EventHandleDelegate<HealDealtEventArgs> action)
{
    var handler = new EventHandle<HealDealtEventArgs>(action, () => HealDealt -= action);
    HealDealt += handler.Invoke;
    return handler;
} 
    #endregion Code for event HealDealt
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
/// (object sender, RequestPlayCardEventArgs) args)
/// </summary>
public class OnRequestPlayCardAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((CardEventsBase)events).SubscribeToRequestPlayCard(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(RequestPlayCardEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, RequestPlayCardEventArgs) args)");
        }
        return ((CardEventsBase)events).SubscribeToRequestPlayCard(delegate(object sender, RequestPlayCardEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void RequestPlayCardEvent (object sender, RequestPlayCardEventArgs args);

    public class RequestPlayCardEventArgs {        public  IEntity CardId { get; }
        public  IEntity Target { get; }
        public  List<string> Blockers { get; set;} 
=new List<string>();        public  RequestPlayCardEventArgs (IEntity CardId, IEntity Target   ){
                  this.CardId = CardId; 
              this.Target = Target; 
}

        }/// <summary>
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
        public  bool IsFree { get; }
        public  CardPlayedEventArgs (IEntity CardId, IEntity Target, bool IsFree   ){
                  this.CardId = CardId; 
              this.Target = Target; 
              this.IsFree = IsFree; 
}

        }/// <summary>
/// (object sender, CardCreatedEventArgs) args)
/// </summary>
public class OnCardCreatedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((CardEventsBase)events).SubscribeToCardCreated(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(CardCreatedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, CardCreatedEventArgs) args)");
        }
        return ((CardEventsBase)events).SubscribeToCardCreated(delegate(object sender, CardCreatedEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void CardCreatedEvent (object sender, CardCreatedEventArgs args);

    public class CardCreatedEventArgs {        public  IEntity CardId { get; }
        public  CardCreatedEventArgs (IEntity CardId   ){
                  this.CardId = CardId; 
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
/// (object sender, CardExhaustedEventArgs) args)
/// </summary>
public class OnCardExhaustedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((CardEventsBase)events).SubscribeToCardExhausted(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(CardExhaustedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, CardExhaustedEventArgs) args)");
        }
        return ((CardEventsBase)events).SubscribeToCardExhausted(delegate(object sender, CardExhaustedEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void CardExhaustedEvent (object sender, CardExhaustedEventArgs args);

    public class CardExhaustedEventArgs {        public  IEntity CardId { get; }
        public  CardExhaustedEventArgs (IEntity CardId   ){
                  this.CardId = CardId; 
}

        }/// <summary>
/// (object sender, RequestDamageMultipliersEventArgs) args)
/// </summary>
public class OnRequestDamageMultipliersAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((CardEventsBase)events).SubscribeToRequestDamageMultipliers(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(RequestDamageMultipliersEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, RequestDamageMultipliersEventArgs) args)");
        }
        return ((CardEventsBase)events).SubscribeToRequestDamageMultipliers(delegate(object sender, RequestDamageMultipliersEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void RequestDamageMultipliersEvent (object sender, RequestDamageMultipliersEventArgs args);

    public class RequestDamageMultipliersEventArgs {        public  int Amount { get; }
        public  IEntity Source { get; }
        public  IEntity Target { get; }
        public  List<float> Multiplier { get; set;} 
=new List<float>();        public  RequestDamageMultipliersEventArgs (int Amount, IEntity Source, IEntity Target   ){
                  this.Amount = Amount; 
              this.Source = Source; 
              this.Target = Target; 
}

        }/// <summary>
/// (object sender, RequestDamageReductionEventArgs) args)
/// </summary>
public class OnRequestDamageReductionAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((CardEventsBase)events).SubscribeToRequestDamageReduction(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(RequestDamageReductionEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, RequestDamageReductionEventArgs) args)");
        }
        return ((CardEventsBase)events).SubscribeToRequestDamageReduction(delegate(object sender, RequestDamageReductionEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void RequestDamageReductionEvent (object sender, RequestDamageReductionEventArgs args);

    public class RequestDamageReductionEventArgs {        public  int Amount { get; }
        public  IEntity Source { get; }
        public  IEntity Target { get; }
        public  List<int> Reduction { get; set;} 
=new List<int>();        public  RequestDamageReductionEventArgs (int Amount, IEntity Source, IEntity Target   ){
                  this.Amount = Amount; 
              this.Source = Source; 
              this.Target = Target; 
}

        }/// <summary>
/// (object sender, CardPlayFailedEventArgs) args)
/// </summary>
public class OnCardPlayFailedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((CardEventsBase)events).SubscribeToCardPlayFailed(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(CardPlayFailedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, CardPlayFailedEventArgs) args)");
        }
        return ((CardEventsBase)events).SubscribeToCardPlayFailed(delegate(object sender, CardPlayFailedEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void CardPlayFailedEvent (object sender, CardPlayFailedEventArgs args);

    public class CardPlayFailedEventArgs {        public  List<string> Reasons { get; }
        public  CardPlayFailedEventArgs (List<string> Reasons   ){
                  this.Reasons = Reasons; 
}

        }/// <summary>
/// (object sender, RequestHealEventArgs) args)
/// </summary>
public class OnRequestHealAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((CardEventsBase)events).SubscribeToRequestHeal(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(RequestHealEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, RequestHealEventArgs) args)");
        }
        return ((CardEventsBase)events).SubscribeToRequestHeal(delegate(object sender, RequestHealEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void RequestHealEvent (object sender, RequestHealEventArgs args);

    public class RequestHealEventArgs {        public  int Amount { get; }
        public  IEntity Source { get; }
        public  IEntity Target { get; }
        public  List<float> Multiplier { get; set;} 
=new List<float>();        public  List<int> Clamps { get; set;} 
=new List<int>();        public  RequestHealEventArgs (int Amount, IEntity Source, IEntity Target   ){
                  this.Amount = Amount; 
              this.Source = Source; 
              this.Target = Target; 
}

        }/// <summary>
/// (object sender, ChooseCardsToDiscardEventArgs) args)
/// </summary>
public class OnChooseCardsToDiscardAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((CardEventsBase)events).SubscribeToChooseCardsToDiscard(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(ChooseCardsToDiscardEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, ChooseCardsToDiscardEventArgs) args)");
        }
        return ((CardEventsBase)events).SubscribeToChooseCardsToDiscard(delegate(object sender, ChooseCardsToDiscardEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void ChooseCardsToDiscardEvent (object sender, ChooseCardsToDiscardEventArgs args);

    public class ChooseCardsToDiscardEventArgs {        public  int Amount { get; }
        public  IEntity Source { get; }
        public  ChooseCardsToDiscardEventArgs (int Amount, IEntity Source   ){
                  this.Amount = Amount; 
              this.Source = Source; 
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
/// (object sender, HealDealtEventArgs) args)
/// </summary>
public class OnHealDealtAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, object instance, EventsBase events)
    {
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((CardEventsBase)events).SubscribeToHealDealt(delegate
            {
                attached.Invoke(instance, Array.Empty<object>());
            });
        }
        if(parameters[0].ParameterType != typeof(object) ||
        parameters[1].ParameterType != typeof(HealDealtEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, HealDealtEventArgs) args)");
        }
        return ((CardEventsBase)events).SubscribeToHealDealt(delegate(object sender, HealDealtEventArgs args)
        {
            attached.Invoke(instance, new[] { sender, args });
        });
    }


}
    //public delegate void HealDealtEvent (object sender, HealDealtEventArgs args);

    public class HealDealtEventArgs {        public  IEntity EntityId { get; }
        public  IEntity SourceEntityId { get; }
        public  int Amount { get; }
        public  HealDealtEventArgs (IEntity EntityId, IEntity SourceEntityId, int Amount   ){
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

    

 
