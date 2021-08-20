using System;
using Data;
using UnityEngine;

public abstract class Proxy<T> : MonoBehaviour where T : GameEntity
{
    public T GameEntity { get; private set; }

    public void Initialize(T entity)
    {
        if (GameEntity != null)
        {
            throw new NotSupportedException("Attempted to initialize Proxy that was already initialized!");
        }
        GameEntity = entity;
        OnInitialize();
    }

    protected abstract void OnInitialize();
}