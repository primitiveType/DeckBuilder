using SummerJam1.Cards;
using UnityEngine;

namespace SummerJam1
{
    public class FaceDownComponentView : ShowIfHasComponentView<FaceDown>
    {
        [SerializeField] private GameObject m_ToToggle;

        protected override void Enable(bool enable)
        {
            base.Enable(enable);
            m_ToToggle.SetActive(!enable);
        }
    }
}
