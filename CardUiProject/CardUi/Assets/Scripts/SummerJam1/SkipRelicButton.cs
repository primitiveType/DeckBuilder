using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class SkipRelicButton : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => { GameContext.Instance.Game.RelicPrizePile.ChoosePrize(null); });
        }
    }
}