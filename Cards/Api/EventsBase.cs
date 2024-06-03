

// ReSharper disable RedundantUsingDirective
// ReSharper disable PossibleNullReferenceException
// ReSharper disable InconsistentNaming
// ReSharper disable RedundantCast
using System;
using System.Collections.Generic;
using System.Reflection;
using Api;


namespace Api{
public abstract class EventsBase : Object{
    #region Code for event EntityCreated
private event EventHandleDelegate<EntityCreatedEventArgs> EntityCreated;
public virtual void OnEntityCreated(EntityCreatedEventArgs args)
{
    EntityCreated?.Invoke(this, args);
}

public EventHandle<EntityCreatedEventArgs> SubscribeToEntityCreated(EventHandleDelegate<EntityCreatedEventArgs> action)
{
    var handler = new EventHandle<EntityCreatedEventArgs>(action, () => EntityCreated -= action);
    EntityCreated += handler.Invoke;
    return handler;
} 
    #endregion Code for event EntityCreated
}
/// <summary>
/// (object sender, EntityCreatedEventArgs) args)
/// </summary>
public class OnEntityCreatedAttribute : EventsBaseAttribute {
    public override IDisposable GetEventHandle(MethodInfo attached, IEventfulComponent instance, EventsBase events)
    {
        instance.EventEntrance.Add(Id, 0);
        var parameters = attached.GetParameters();
        if (parameters.Length == 0)
        {
            return ((EventsBase)events).SubscribeToEntityCreated(delegate
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
        parameters[1].ParameterType != typeof(EntityCreatedEventArgs)){
            throw new NotSupportedException("Wrong parameters for attribute usage! must match signature (object sender, EntityCreatedEventArgs) args)");
        }
        return ((EventsBase)events).SubscribeToEntityCreated(delegate(object sender, EntityCreatedEventArgs args)
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
    //public delegate void EntityCreatedEvent (object sender, EntityCreatedEventArgs args);

    public class EntityCreatedEventArgs {        public  IEntity Entity { get; }
        public  EntityCreatedEventArgs (IEntity Entity   ){
                  this.Entity = Entity; 
}

        }




 }

    

 
