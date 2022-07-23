using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class RelicPrizePileButton : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => { GameContext.Instance.Game.RelicPrizePile.SetupPrizePile(); });
        }
    }
}
