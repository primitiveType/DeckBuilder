using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class GainEnergyButton : MonoBehaviour
    {
        public Button Button { get; set; }

        private void Awake()
        {
            Button = GetComponent<Button>();
            Button.onClick.AddListener(GrantEnergy);
        }

        private void GrantEnergy()
        {
            GameContext.Instance.Game.Player.CurrentEnergy++;
        }
    }
}