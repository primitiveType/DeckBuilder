using App;
using TMPro;
using UnityEngine.UI;

namespace SummerJam1
{
    public class BuyButton : View<IBuyable>
    {
        private Button Button { get; set; }
        private Text Text { get; set; }
        private void Awake()
        {
            Button = GetComponent<Button>();
            Text = GetComponentInChildren<Text>();
            Button.onClick.AddListener(ButtonClicked);
        }

        private void ButtonClicked()
        {
            Model.Buy();
        }


        [PropertyListener(nameof(IBuyable.Cost))]
        private void OnCostChanged()
        {
            Text.text = $"Buy ({Model.Cost})";
        }
    }
}
