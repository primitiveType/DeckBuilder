using System;
using App.Utility;
using UnityEngine;

namespace SummerJam1
{
    public class SummerJam1CardFactory : MonoBehaviourSingleton<SummerJam1CardFactory>
    {
        [SerializeField] private GameObject m_CardPrefab;
        public GameObject CardPrefab => m_CardPrefab;
        [SerializeField] private GameObject m_BlackPepper;

        [SerializeField] private GameObject m_SpicyPepper;

        [SerializeField] private GameObject m_PrepTalk;

        public GameObject GetRenderer(SummerJam1CardAsset value)
        {
            switch (value)
            {
                case SummerJam1CardAsset.BlackPepper:
                    return Instantiate(m_BlackPepper);
                case SummerJam1CardAsset.SpicyPepper:
                    return Instantiate(m_SpicyPepper);
                case SummerJam1CardAsset.PrepTalk:
                    return Instantiate(m_PrepTalk);
                default:
                    Debug.LogWarning($"No prefab found for {value}. Using black pepper as fallback.");
                    return Instantiate(m_BlackPepper);

            }
        }
    }
}