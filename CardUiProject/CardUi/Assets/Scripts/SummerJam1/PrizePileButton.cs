using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class PrizePileButton : MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(()=> {GameContext.Instance.Game.PrizePile.SetupRandomPrizePile();});
        }
    }
}
