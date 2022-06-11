using Newtonsoft.Json;
using UnityEngine;
using Component = Api.Component;

namespace App
{
    public class PileViewBridge : Component, IGameObject
    { //piles are pre-determined so there is much less logic.
        [JsonIgnore] public GameObject gameObject { get; set; }
    }
}

