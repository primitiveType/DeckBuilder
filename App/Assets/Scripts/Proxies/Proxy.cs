using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.GameEntities;
using UnityEngine;

public class Proxy : Proxy<IGameEntity>
{
}

public abstract class EntityBehaviour<T> : MonoBehaviour where T : IGameEntity
{
    public T GameEntity { get; private set; }

    public void Initialize(T entity)
    {
        if (GameEntity != null)
        {
            throw new NotSupportedException("Attempted to initialize Proxy that was already initialized!");
        }

        GameEntity = entity;
        AttachListeners();
        OnInitialize();
    }

    protected abstract void OnInitialize();

    private void AttachListeners()
    {
        foreach (PropertyListenerInfo method in ReflectionService.GetPropertyListeners(GetType()))
        {
            Action<object, PropertyChangedEventArgs> action = (sender, args) =>
            {
                try
                {
                    if (method.Filter == null || method.Filter == args.PropertyName)
                    {
                        method.MethodInfo.Invoke(this, new[] { sender, args });
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Caught exception executing event! {e.Message}", this);
                }
            };
            GameEntity.AddListener(action);
            method.MethodInfo.Invoke(this, new object[] { this, new PropertyChangedEventArgs(method.Filter) });
        }
    }
}

public abstract class Proxy<T> : EntityBehaviour<T>, IGameEntityProperty, IProxy where T : class, IGameEntity
{
    IGameEntity IGameEntityProperty.GameEntity => GameEntity;

    protected override void OnInitialize()
    {
        var components = gameObject.GetComponents<IProxyComponent>();
        GameEntity.DestroyedEvent += OnEntityDestroyed;
        foreach (var component in components)
        {
            component.Initialize(GameEntity);
        }
    }

    protected virtual void OnEntityDestroyed(object sender, EntityDestroyedArgs args)
    {
        if(gameObject)
        Destroy(gameObject);
    }

    public void Initialize(IGameEntity entity)
    {
        base.Initialize((T)entity);
    }
}

public class PropertyListenerAttribute : Attribute
{
    public readonly string m_NameFilter;

    public PropertyListenerAttribute(string nameFilter = null)
    {
        m_NameFilter = nameFilter;
    }
}

public struct PropertyListenerInfo
{
    public PropertyListenerInfo(MethodInfo methodInfo, string filter)
    {
        MethodInfo = methodInfo;
        Filter = filter;
    }

    public MethodInfo MethodInfo { get; }
    public string Filter { get; }
}

public static class ReflectionService
{
    private static readonly Dictionary<Type, List<PropertyListenerInfo>> PropertyListeners =
        new Dictionary<Type, List<PropertyListenerInfo>>();
    private static Dictionary<FieldInfo, IReadOnlyList<Attribute>> FieldAttributes { get; } = new Dictionary<FieldInfo, IReadOnlyList<Attribute>>();

    private static Dictionary<Type, IReadOnlyList<FieldInfo>> TypeMemberFields { get; } = new Dictionary<Type, IReadOnlyList<FieldInfo>>();

    public static IReadOnlyList<FieldInfo> GetTypeMemberFields(Type type) {
        if (!TypeMemberFields.TryGetValue(type, out IReadOnlyList<FieldInfo> fields)) {
            fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (FieldInfo fieldInfo in fields) {
                GetFieldAttributes(fieldInfo);
            }

            TypeMemberFields.Add(type, fields);
        }
        return fields;
    }

    public static T GetFieldAttribute<T>(FieldInfo field) where T : Attribute {
        return (T) GetFieldAttributes(field).FirstOrDefault(attribute => attribute is T);
    }

    private static IReadOnlyList<Attribute> GetFieldAttributes(FieldInfo info) {
        if (!FieldAttributes.TryGetValue(info, out IReadOnlyList<Attribute> attributes)) {
            attributes = (IReadOnlyList<Attribute>) info.GetCustomAttributes(true).ToList();
            FieldAttributes.Add(info, attributes);
        }
        return attributes;
    }

    public static List<PropertyListenerInfo> GetPropertyListeners(Type type)
    {
        if (PropertyListeners.ContainsKey(type))
        {
            return PropertyListeners[type];
        }

        List<PropertyListenerInfo> methods = new List<PropertyListenerInfo>();
        foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {

            foreach (var attribute in method.GetCustomAttributes(true))
            {
                if (attribute is PropertyListenerAttribute propertyListenerAttribute)
                {
                    methods.Add(new PropertyListenerInfo(method, propertyListenerAttribute.m_NameFilter));
                }
            }
        }

        PropertyListeners[type] = methods;

        return methods;
    }
}