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

        [SerializeField] private GameObject m_BlackPepper;

        [SerializeField] private GameObject m_SpicyPepper;

        [SerializeField] private GameObject m_PrepTalk;
        [SerializeField] private GameObject m_Butter;
        [SerializeField] private GameObject m_Beef;
        [SerializeField] private GameObject m_Cheddar;
        [SerializeField] private GameObject m_HerbsAndSpices;
        [SerializeField] private GameObject m_Starter;
        [SerializeField] private GameObject m_Dice;
        [SerializeField] private GameObject m_Soup;

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
                case SummerJam1CardAsset.HerbsAndSpices:
                    return Instantiate(m_HerbsAndSpices);
                case SummerJam1CardAsset.Wasabi:
                case SummerJam1CardAsset.Potatoes:
                case SummerJam1CardAsset.Milk:
                case SummerJam1CardAsset.Starter:
                    return Instantiate(m_Starter);
                case SummerJam1CardAsset.Soup:
                    return Instantiate(m_Soup);
                case SummerJam1CardAsset.Cheddar:
                    return Instantiate(m_Cheddar);
                case SummerJam1CardAsset.Dice:
                    return Instantiate(m_Dice);
                case SummerJam1CardAsset.Tofu:
                case SummerJam1CardAsset.ApronPockets:
                case SummerJam1CardAsset.Beef:
                    return Instantiate(m_Beef);
                case SummerJam1CardAsset.Butter:
                    return Instantiate(m_Butter);
                default:
                    Debug.LogWarning($"No prefab found for {value}. Using black pepper as fallback.");
                    return Instantiate(m_BlackPepper);
            }
        }
    }
}
