using System;
using App.Utility;
using UnityEngine;

namespace SummerJam1
{
    public class SummerJam1CardFactory : MonoBehaviourSingleton<SummerJam1CardFactory>
    {
        [SerializeField] private GameObject m_CardPrefab;
        public GameObject CardPrefab => m_CardPrefab;
        [SerializeField] private GameObject m_RelicPrefab;

        public GameObject RelicPrefab => m_RelicPrefab;
    }
}
