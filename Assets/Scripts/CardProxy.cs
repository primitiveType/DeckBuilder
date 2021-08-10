using Data;
using UnityEngine;

public class CardProxy : Proxy<Card>
{
    protected override void OnInitialize()
    {
        Debug.Log($"Initialized card proxy with id {GameEntity.Id}.");
        
    }
}