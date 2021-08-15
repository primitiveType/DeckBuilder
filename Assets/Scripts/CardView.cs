using Data;
using UnityEngine;

public class CardView : View<Card>
{
    protected override void OnInitialize()
    {
        Debug.Log($"Initialized card proxy with id {GameEntity.Id}.");
        
    }
}