using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SummerJam1
{
    public class StartOverButton : MonoBehaviour
    {
        [SerializeField] private Button m_Button;
        private void Start()
        {
            m_Button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            GameContext.Instance.StartOver();
        }
    }
}
