using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class SkipRelicButton : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => { SummerJam1Context.Instance.Game.RelicPrizePile.ChoosePrize(null); });
        }
    }
}