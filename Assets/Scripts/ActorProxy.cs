using Data;
using UnityEngine;

public class ActorProxy : Proxy<Actor>
{
    protected override void OnInitialize()
    {
        Debug.Log($"Initialized actor proxy with id {GameEntity.Id}.");
    }
}