using App;
using CardsAndPiles;
using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class SelectDungeonButton : View<DungeonPile>, ISetModel
    {
        [SerializeField] private Button m_Button;

        protected override void Start()
        {
            base.Start();
            m_Button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            DungeonViewManager.Instance.ViewDungeon(Model);
        }
    }
}
