using UnityEngine;

namespace SummerJam1
{
    public class SummerJam1CardFactory : MonoBehaviourSingleton<SummerJam1CardFactory>
    {
        [SerializeField] private GameObject m_CardPrefab;
        public GameObject CardPrefab => m_CardPrefab;
    }
}