using System.ComponentModel;
using CardsAndPiles.Components;
using TMPro;
using UnityEngine;

namespace App
{
    public class NameView : ComponentView<NameComponent>
    {
        [SerializeField] private TMP_Text Text;
        
        protected override void ComponentOnPropertyChanged()
        {
            Text.text = Component.Value;
        }
    }
    
}