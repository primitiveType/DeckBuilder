using System.ComponentModel;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;
using JetBrains.Annotations;
using UnityEngine;

public abstract class ActorProxy<TActor> : Proxy<TActor> where TActor : class, IActor, IGameEntity
{
    protected override void OnInitialize()
    {
        base.OnInitialize();
        GameEntity.Context.Events.DamageDealt += OnDamageDealt;
        GameEntity.Context.Events.ActorDied += OnActorDied;
    }


    private void OnActorDied(object sender, ActorDiedEventArgs args)
    {
        if (args.Actor.Id == GameEntity.Id)
        {
            Destroy(gameObject);
        }
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
        GameEntity.Context.Events.ActorDied -= OnActorDied;
    }
}