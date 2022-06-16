using System;
using App.Utility;
using UnityEngine;

namespace SummerJam1
{
    public class SummerJam1UnitFactory : MonoBehaviourSingleton<SummerJam1UnitFactory>
    {
        [SerializeField] private GameObject m_IceCream;
        [SerializeField] private GameObject m_HeadCheese;
        [SerializeField] private GameObject m_Sandwich;
        [SerializeField] private GameObject m_Noodles;
        [SerializeField] private GameObject m_Tofu;
        [SerializeField] private GameObject m_Player;
        [SerializeField] private GameObject m_Starter;
        [SerializeField] private GameObject m_Meatbaby;
        [SerializeField] private GameObject m_Donut;
        [SerializeField] private GameObject m_Soup;
        [SerializeField] private GameObject m_Pie;
        [SerializeField] private GameObject m_UnitPrefab;
        public GameObject UnitPrefab => m_UnitPrefab;

        public GameObject GetInstance(SummerJam1UnitAsset value)
        {
            switch (value)
            {
                case SummerJam1UnitAsset.IceCream:
                    return Instantiate(m_IceCream);
                case SummerJam1UnitAsset.HeadCheese:
                    return Instantiate(m_HeadCheese);
                case SummerJam1UnitAsset.Sandwich:
                    return Instantiate(m_Sandwich);
                case SummerJam1UnitAsset.Noodles:
                    return Instantiate(m_Noodles);
                case SummerJam1UnitAsset.Tofu:
                    return Instantiate(m_Tofu);
                case SummerJam1UnitAsset.Player:
                    return Instantiate(m_Player);
                case SummerJam1UnitAsset.Starter:
                    return Instantiate(m_Starter);
                case SummerJam1UnitAsset.Meatloaf:
                    return Instantiate(m_Meatbaby);
                case SummerJam1UnitAsset.Donut:
                    return Instantiate(m_Donut);
                case SummerJam1UnitAsset.Soup:
                    return Instantiate(m_Soup);
                case SummerJam1UnitAsset.Pie:
                    return Instantiate(m_Pie);
                default:
                    Debug.LogError($"No prefab found for {value}. Using sandwich as fallback.");
                    return Instantiate(m_Sandwich);
                    throw new ArgumentOutOfRangeException(nameof(value), value, "No prefab found!");
            }
        }
    }
}