using System;
using UnityEngine;

namespace SummerJam1
{
    public class SummerJam1UnitFactory : MonoBehaviourSingleton<SummerJam1UnitFactory>
    {
        [SerializeField] private GameObject m_IceCream;
        [SerializeField] private GameObject m_HeadCheese;
        [SerializeField] private GameObject m_Sandwich;
        [SerializeField] private GameObject m_Noodles;

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
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, "No prefab found!");
            }
        }
    }
}