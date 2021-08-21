using System;
using Data;
using UnityEngine;

public abstract class ActorProxy : Proxy<Actor>
{
    protected override void OnInitialize()
    {
        Debug.Log($"Initialized actor proxy with id {GameEntity.Id}.");
        GameEntity.Context.Events.DamageDealt += OnDamageDealt;
    }

    private void OnDamageDealt(object sender, DamageDealtArgs args)
    {
        if (GameEntity.Id == args.ActorId)
        {
            DamageReceived();
        }
    }

    public abstract void DamageReceived();

    protected virtual void OnDestroy()
    {
        GameEntity.Context.Events.DamageDealt -= OnDamageDealt;
    }
}