using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class SkipPrizesButton : MonoBehaviour
    {
        private void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(TakePrizes);
        }

        private void TakePrizes()
        {
            SummerJam1Context.Instance.Game.PrizePile.Clear();
        }
    }
}