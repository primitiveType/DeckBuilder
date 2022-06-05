using UnityEngine;

namespace SummerJam1
{
    public class
        SummerJam1UnitViewBridge : Api.Component, IGameObject //ViewBridge<SummerJam1Card, SummerJam1CardViewBridge>
    {
        // public override GameObject Prefab => SummerJam1Helper.Instance.UnitPrefab;
        public GameObject gameObject { get; set; }
    }
}