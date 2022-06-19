using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class RelicPrizePileButton : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(() => { SummerJam1Context.Instance.Game.RelicPrizePile.SetupPrizePile(); });
        }
    }
}
