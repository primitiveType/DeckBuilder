using App;
using CardsAndPiles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SummerJam1
{
    public class DungeonPreviewView : View<DungeonPile>
    {
        [SerializeField] private TMP_Text RewardText;
        [SerializeField] private TMP_Text EnemyCountText;
        [SerializeField] private TMP_Text DifficultyText;
        [SerializeField] private TMP_Text OtherText;
        [SerializeField] private Button StartDungeonButton;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            EnemyCountText.text = $"{Entity.Children.Count} cards in dungeon.";
            OtherText.text = $"Objective : {Model.Description}";
            RewardText.text = $"Reward : {Entity.GetComponent<IReward>()?.RewardText}";
            StartDungeonButton.onClick.AddListener(StartDungeon);
            DifficultyText.text = $"Difficulty: { Model.Difficulty.ToString()}";
        }

        private void StartDungeon()
        {
            GameContext.Instance.Game.StartBattle(Model);
        }
    }
}
