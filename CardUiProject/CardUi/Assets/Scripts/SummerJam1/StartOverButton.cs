using UnityEngine;
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
            SummerJam1Context.Instance.StartOver();
        }
    }
}