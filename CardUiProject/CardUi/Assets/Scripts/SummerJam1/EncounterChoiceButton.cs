using System;
using App;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class EncounterChoiceButton : ComponentView<EncounterChoice>
    {
        [SerializeField] private Button m_Button;

        protected override void Start()
        {
            base.Start();
            m_Button.onClick.AddListener(OnClick);
            SetText();
        }

        protected override void ComponentOnPropertyChanged()
        {
            //assign text to button.
            SetText();
        }

        private void SetText()
        {
            switch (Component)
            {
                case BattleChoice battleChoice:
                    m_Button.GetComponentInChildren<TMP_Text>().text = "Battle Normal Enemies for a booster pack.";
                    break;
                case ShopChoice shopChoice:
                    m_Button.GetComponentInChildren<TMP_Text>().text = "Sell treasure and spend gold at the shop.";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Component));
            }
        }

        private void OnClick()
        {
            switch (Component)
            {
                case BattleChoice battleChoice:
                    GameContext.Instance.Game.StartBattle();
                    break;
                case ShopChoice shopChoice:
                    GameContext.Instance.Game.StartShop();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Component));
            }
                
          
        }
    }
}
