using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class StartBattleButton : MonoBehaviour
    {

        public void SetBattleInfo(int info)
        {
            //set info.
            Info = info;
        }

        public int Info { get; set; }

        [SerializeField] private Button m_Button;
        private void Start()
        {
            m_Button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            GameContext.Instance.StartBattle(Info);
        }
    }
}