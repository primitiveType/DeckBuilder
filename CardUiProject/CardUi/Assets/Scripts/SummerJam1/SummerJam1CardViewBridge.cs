using Common;
using SummerJam1.Cards;
using UnityEngine;

namespace SummerJam1
{
    public class SummerJam1CardViewBridge : ViewBridge<SummerJam1Card, SummerJam1CardViewBridge>
    {
        public override GameObject Prefab => SummerJam1Helper.Instance.CardPrefab;
    }
}