using Newtonsoft.Json;
using UnityEngine;

namespace SummerJam1
{
    public class
        SummerJam1UnitViewBridge : Api.Component, IGameObject //ViewBridge<SummerJam1Card, SummerJam1CardViewBridge>
    {
        [JsonIgnore] public GameObject gameObject { get; set; }
    }
}