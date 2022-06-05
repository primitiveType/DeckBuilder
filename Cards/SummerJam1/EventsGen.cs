
// ReSharper disable RedundantUsingDirective
// ReSharper disable PossibleNullReferenceException
// ReSharper disable InconsistentNaming

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

    public class UnitCreatedEventArgs {        public  IEntity EntityId { get; }
        public  UnitCreatedEventArgs (IEntity EntityId   ){
                  this.EntityId = EntityId; 
}

  }



//TODO: generate attributes with static override functions that get the event handle from a game context.
//TODO: add generation of invoke functions.? 

 }

    

 