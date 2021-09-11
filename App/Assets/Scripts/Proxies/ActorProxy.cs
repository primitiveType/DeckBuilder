using DeckbuilderLibrary.Data;
using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using DeckbuilderLibrary.Data.GameEntities.Actors;

public abstract class ActorProxy<TActor> : Proxy<TActor> where TActor : IActor, IGameEntity
{
    protected override void OnInitialize()
    {
        GameEntity.Context.Events.DamageDealt += OnDamageDealt;
        GameEntity.Context.Events.ActorsSwapped += OnActorsSwapped;
        GameEntity.Context.Events.ActorDied += OnActorDied;
    }

    private void OnActorDied(object sender, ActorDiedEventArgs args)
    {
        if (args.Actor.Id == GameEntity.Id)
        {
            Destroy(gameObject);
        }
    }

    private void OnActorsSwapped(object sender, ActorsSwappedEventArgs args)
    {
        if (args.Actor1?.Id == GameEntity.Id || args.Actor2?.Id == GameEntity.Id)
        {
            var node = GameEntity.Context.GetCurrentBattle().Graph.GetNodeOfActor(GameEntity);
            var battle = GetComponentInParent<BattleProxy>();
            if (battle) //this is a HACK. Just because playerProxy is a weird exception right now.
            {
                var proxyNode = battle.GetNodeProxyByEntity(node);
                this.transform.position = proxyNode.transform.position;
            }
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
        GameEntity.Context.Events.ActorsSwapped -= OnActorsSwapped;
        GameEntity.Context.Events.ActorDied -= OnActorDied;
    }
}