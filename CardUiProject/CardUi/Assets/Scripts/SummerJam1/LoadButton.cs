using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class LoadButton : MonoBehaviour
    {
        [SerializeField] private Button m_Button;
        private void Start()
        {
            m_Button.onClick.AddListener(OnClick);
            gameObject.SetActive(GameContext.Instance.HasSaveData());
        }

        private void OnClick()
        {
            GameContext.Instance.LoadGame();
        }
    }
}
