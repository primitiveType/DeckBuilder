using UnityEngine;

public class PileProxy : Proxy<Pile>
{//I think this thing would probably only care about how many cards are in it... and might have inheritors for different
    //types of piles.
    protected override void OnInitialize()
    {
        Debug.Log($"Initialized pile proxy with id {GameEntity.Id}.");
    }
}