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
    }
}
