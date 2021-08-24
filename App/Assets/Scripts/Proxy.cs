﻿using System;
using DeckbuilderLibrary.Data.GameEntities;
using UnityEngine;

public abstract class Proxy<T> : MonoBehaviour where T : IGameEntity
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