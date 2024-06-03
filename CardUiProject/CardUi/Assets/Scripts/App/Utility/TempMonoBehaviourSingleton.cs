using UnityEngine;

namespace SummerJam1
{
    public class TempMonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {

        public static T Instance { get; set; }

        private void Awake()
        {
            Instance = GetComponent<T>();
        }

        private void OnDestroy()
        {
            Instance = default(T);
        }
    }
}