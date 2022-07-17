using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class StartGameButton : MonoBehaviour
    {
        [SerializeField] private Button m_Button;
        private void Start()
        {
            m_Button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            SummerJam1Context.Instance.StartGame();
        }
    }
}