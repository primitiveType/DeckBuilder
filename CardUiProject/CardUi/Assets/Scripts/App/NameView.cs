using System.ComponentModel;
using CardsAndPiles.Components;
using Common;
using TMPro;
using UnityEngine;

namespace SummerJam1
{
    public class NameView : View<NameComponent>
    {
        [SerializeField] private TMP_Text Text;
        
        [PropertyListener]
        private void OnNameChanged(object sender, PropertyChangedEventArgs args)
        {
            Text.text = Model.Value;
        }
    }
    
}