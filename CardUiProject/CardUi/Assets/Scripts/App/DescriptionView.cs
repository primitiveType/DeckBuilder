using System.ComponentModel;
using CardsAndPiles.Components;
using Common;
using TMPro;
using UnityEngine;

namespace SummerJam1
{
    public class DescriptionView : View<IDescription>
    {
        [SerializeField] private TMP_Text Text;
        
        [PropertyListener(nameof(IDescription.Description))]
        private void OnDescriptionChanged(object sender, PropertyChangedEventArgs args)
        {
            Text.text = Model.Description;
        }
    }
}