using System;
using App;
using App.Utility;
using CardsAndPiles;
using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class StartBattleButton : MonoBehaviour
    {
        [SerializeField] private Button m_Button;

        protected void Start()
        {
            m_Button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            GameContext.Instance.Game.StartBattle();
        }
    }
}
