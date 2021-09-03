using DeckbuilderLibrary.Data.Events;
using DeckbuilderLibrary.Data.GameEntities;
using UnityEngine;

public class IntentProxy : Proxy<Intent>
{
    private int OwnerId { get; set; }

    protected override void OnInitialize()
    {
        OwnerId = GameEntity.OwnerId;
        GameEntity.Context.Events.IntentChanged += OnIntentChanged;
        GameEntity.Context.Events.ActorDied += OnActorDied;
        var r = GetComponentInChildren<Renderer>();
        if (GameEntity is DamageIntent)
        {
            r.material.color = Color.red;
        }
        else
        {
            r.material.color = Color.yellow;
        }
    }

    private void OnActorDied(object sender, ActorDiedEventArgs args)
    {
        if (args.Actor.Id == OwnerId)
        {
            Destroy(gameObject);
        }
    }


    private void OnIntentChanged(object sender, IntentChangedEventArgs args)
    { 
        if (args.Owner.Id == OwnerId && args.Owner.Intent != GameEntity)
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        GameEntity.Context.Events.IntentChanged -= OnIntentChanged;
    }
}

