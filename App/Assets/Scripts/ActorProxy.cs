using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using UnityEngine;

public abstract class ActorProxy<TActor> : Proxy<TActor> where TActor : IGameEntity
{
    protected override void OnInitialize()
    {
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