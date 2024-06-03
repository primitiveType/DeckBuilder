using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class SaveButton : MonoBehaviour
    {
        [SerializeField] private Button m_Button;
        private void Start()
        {
            m_Button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            GameContext.Instance.SaveGame();
        }
    }
}