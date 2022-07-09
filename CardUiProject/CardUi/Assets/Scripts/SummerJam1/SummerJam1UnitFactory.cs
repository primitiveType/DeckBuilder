using System;
using App.Utility;
using UnityEngine;

namespace SummerJam1
{
    public class SummerJam1UnitFactory : MonoBehaviourSingleton<SummerJam1UnitFactory>
    {
        [SerializeField] private GameObject m_UnitPrefab;
        public GameObject UnitPrefab => m_UnitPrefab;
    }
}
