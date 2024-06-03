using System;
using App;
using App.Utility;
using CardsAndPiles;
using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class StartBattleButton : View<DungeonPile>, ISetModel
    {
        [SerializeField] private Button m_Button;

        protected override void Start()
        {
            base.Start();
            m_Button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            GameContext.Instance.Game.StartBattle(Model);
        }
    }
}
