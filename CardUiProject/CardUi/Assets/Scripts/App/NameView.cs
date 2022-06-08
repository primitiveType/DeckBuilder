using CardsAndPiles.Components;
using Common;
using TMPro;
using UnityEngine;

namespace SummerJam1
{
    public class NameView : View<NameComponent>
    {
        [SerializeField] private TMPro.TMP_Text Text;
        
        [PropertyListener]
        private void OnNameChanged()
        {
            Text.text = Model.Value;
        }
    }
}