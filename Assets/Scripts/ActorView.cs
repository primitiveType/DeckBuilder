using Data;
using UnityEngine;

public class ActorView : View<Actor>
{
    protected override void OnInitialize()
    {
        Debug.Log($"Initialized actor proxy with id {GameEntity.Id}.");
    }
}