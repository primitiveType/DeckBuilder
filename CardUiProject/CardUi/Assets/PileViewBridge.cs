using UnityEngine;
using Component = Api.Component;

public class PileViewBridge : Component, IGameObject
{ //piles are pre-determined so there is much less logic.
    public GameObject gameObject { get; set; }
}