using UnityEngine;

namespace SummerJam1
{
    public class MapTester : MonoBehaviour
    {
        private void Awake()
        {
            GameContext.Instance.StartGame();
        }
    }
}
