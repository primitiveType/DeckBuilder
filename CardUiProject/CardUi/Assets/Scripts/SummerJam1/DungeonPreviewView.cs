using System.Collections.Specialized;
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
            Entity.Children.CollectionChanged += UpdateChildCount;
            UpdateChildCount();
            OtherText.text = $"Objective : {Model.Description}";
            RewardText.text = $"Reward : {Entity.GetComponent<IReward>()?.RewardText}";
            StartDungeonButton.onClick.AddListener(StartDungeon);
            DifficultyText.text = $"Difficulty: {Model.Difficulty.ToString()}";
        }

        private void UpdateChildCount(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateChildCount();
        }

        private void UpdateChildCount()
        {
            EnemyCountText.text = $"{Entity.GetComponentsInChildren<PrefabReference>().Count} cards in dungeon.";
        }

        private void StartDungeon()
        {
            GameContext.Instance.Game.StartBattle(Model);
        }
    }
}
